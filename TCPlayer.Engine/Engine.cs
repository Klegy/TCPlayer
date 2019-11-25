using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ManagedBass;
using ManagedBass.Cd;
using ManagedBass.Fx;
using ManagedBass.Midi;
using ManagedBass.Mix;
using TCPlayer.Engine.Internals;

namespace TCPlayer.Engine
{
    public sealed class Engine : IEngine
    {
        private float _lastvol;
        private int _sourceHandle, _mixerHandle, _fxHandle;
        private readonly int _maxfft;
        private PeakEQParameters _eq;
        private DownloadProcedure _callback;
        private EqualizerConfig _eqConfig;
        private GCHandle _eqHandle;
        private bool _paused;
        private bool _isplaying;
        private ChannelInfo _sourceInfo;
        private readonly IEngineConfiguration _configuration;
        private static readonly string[] _plugins = new string[]
        {
            "bass_aac.dll", "bass_ac3.dll", "bass_ape.dll",
            "bass_mpc.dll", "bass_spx.dll", "bass_tta.dll",
            "bassalac.dll", "bassdsd.dll", "bassflac.dll",
            "bassopus.dll", "basswma.dll", "basswv.dll", "bassmidi.dll",
        };

        private void NotifyPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void InitEq(ref int chHandle, float fGain = 0.0f)
        {
            _eqConfig = new EqualizerConfig();

            // set peaking equalizer effect with no bands
            _fxHandle = Bass.ChannelSetFX(chHandle, EffectType.PeakEQ, 0); // BASS_ChannelSetFX(chan, BASS_FX_BFX_PEAKEQ, 0);

            _eq.fGain = fGain;
            _eq.fQ = EqConstants.fQ;
            _eq.fBandwidth = EqConstants.fBandwidth;
            _eq.lChannel = FXChannelFlags.All;

            // create 1st band for bass
            _eq.lBand = 0;
            _eq.fCenter = EqConstants.fCenter_Bass;
            Bass.FXSetParameters(_fxHandle, _eqHandle.AddrOfPinnedObject());

            // create 2nd band for mid
            _eq.lBand = 1;
            _eq.fCenter = EqConstants.fCenter_Mid;
            Bass.FXSetParameters(_fxHandle, _eqHandle.AddrOfPinnedObject());

            // create 3rd band for treble
            _eq.lBand = 2;
            _eq.fCenter = EqConstants.fCenter_Treble;
            Bass.FXSetParameters(_fxHandle, _eqHandle.AddrOfPinnedObject());

            UpdateFxConfiguration(_eqConfig);
        }

        private void UpdateFxConfiguration(EqualizerConfig eqConfig)
        {
            int band = 0;
            foreach (double value in eqConfig)
            {
                _eq.lBand = band;
                Bass.FXGetParameters(_fxHandle, _eqHandle.AddrOfPinnedObject());
                _eq.fGain = (float)value;
                Bass.FXSetParameters(_fxHandle, _eqHandle.AddrOfPinnedObject());
                ++band;
            }
        }

        private void FreeHandles()
        {
            if (_sourceHandle != 0)
            {
                Bass.StreamFree(_sourceHandle);
                _sourceHandle = 0;
                IsPlaying = false;
            }
            if (_mixerHandle != 0)
            {
                Bass.ChannelRemoveFX(_mixerHandle, _fxHandle);
                Bass.StreamFree(_mixerHandle);
                _mixerHandle = 0;
                IsPlaying = false;
            }
        }

        public Engine(IEngineConfiguration configuration)
        {
            _configuration = configuration;
            foreach (string plugin in _plugins)
            {
                Bass.PluginLoad(plugin);
            }
            _eq = new PeakEQParameters();
            _eqHandle = GCHandle.Alloc(_eq, GCHandleType.Pinned);
            _maxfft = (int)(DataFlags.Available | DataFlags.FFT2048);
        }

        public int CurrentDeviceID
        {
            get;
            private set;
        }

        public EqualizerConfig Equalizer
        {
            get { return _eqConfig; }
            set
            {
                _eqConfig = value;
                UpdateFxConfiguration(_eqConfig);
            }
        }


        public bool IsPaused
        {
            get { return _paused || (_mixerHandle == 0); }
        }

        public MediaKind CurrentMediaKind
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the channel length
        /// </summary>
        public double Length
        {
            get
            {
                var len = Bass.ChannelGetLength(_sourceHandle);
                return Bass.ChannelBytes2Seconds(_sourceHandle, len);
            }
        }

        public double Position
        {
            get
            {
                var pos = Bass.ChannelGetPosition(_sourceHandle);
                return Bass.ChannelBytes2Seconds(_sourceHandle, pos);
            }
            set
            {
                var pos = Bass.ChannelSeconds2Bytes(_sourceHandle, value);
                Bass.ChannelSetPosition(_sourceHandle, pos);
            }
        }

        public float Volume
        {
            get
            {

                float temp = 0.0f;
                Bass.ChannelGetAttribute(_mixerHandle, ChannelAttribute.Volume, out temp);
                return temp;
            }
            set
            {
                Bass.ChannelSetAttribute(_mixerHandle, ChannelAttribute.Volume, value);
                _lastvol = value;
            }
        }

        public bool IsPlaying
        {
            get { return _isplaying; }
            private set
            {
                if (value == _isplaying) return;
                _isplaying = value;
                NotifyPropertyChanged(nameof(IsPlaying));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool GetChannelData(out short[] data, float seconds)
        {
            var length = (int)Bass.ChannelSeconds2Bytes(_mixerHandle, seconds);
            if (length > 0)
            {
                data = new short[length / 2];
                length = Bass.ChannelGetData(_mixerHandle, data, length);
                return length > 0;
            }
            data = null;
            return false;
        }

        public bool GetFFTData(float[] fftDataBuffer)
        {
            return Bass.ChannelGetData(_mixerHandle, fftDataBuffer, _maxfft) > 0;
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            const int length = 2048;
            int num = (int)Math.Round((double)length * (double)frequency / (double)_sourceInfo.Frequency);
            if (num > (length / 2) - 1) num = (length / 2) - 1;
            return num;
        }

        public void PlayPause()
        {
            if (_paused)
            {
                Bass.ChannelPlay(_mixerHandle, false);
                _paused = false;
                IsPlaying = true;
            }
            else
            {
                Bass.ChannelPause(_mixerHandle);
                _paused = true;
                IsPlaying = false;
            }
        }

        public void Stop()
        {
            Bass.ChannelStop(_mixerHandle);
            IsPlaying = false;
            _paused = false;
        }

        public void Load(string url)
        {
            CurrentMediaKind = MediaKind.None;

            FreeHandles();

            if (FormatHelpers.IsMidi(url) && File.Exists(_configuration.SoundFontPath))
            {
                BassMidi.DefaultFont = _configuration.SoundFontPath;
            }

            const BassFlags sourceflags = BassFlags.Decode | BassFlags.Loop | BassFlags.Float | BassFlags.Prescan;
            const BassFlags mixerflags = BassFlags.MixerDownMix | BassFlags.MixerPositionEx | BassFlags.AutoFree;

            if (FormatHelpers.IsNetwork(url))
            {
                _sourceHandle = Bass.CreateStream(url, 0, sourceflags, _callback, IntPtr.Zero);
                CurrentMediaKind = MediaKind.Network;
            }
            else if (FormatHelpers.IsCd(url))
            {
                (int drive, int track) cdInfo = FormatHelpers.ProcessCdUrl(url);
                _sourceHandle = BassCd.CreateStream(cdInfo.drive, cdInfo.track, sourceflags);
                CurrentMediaKind = MediaKind.CDStream;
            }
            else if (FormatHelpers.IsTracker(url))
            {
                _sourceHandle = Bass.MusicLoad(url, 0, 0, sourceflags);
                CurrentMediaKind = MediaKind.Tracker;
            }
            else
            {
                _sourceHandle = Bass.CreateStream(url, 0, 0, sourceflags);
                CurrentMediaKind = MediaKind.File;
            }

            if (_sourceHandle == 0)
                ExceptionFactory.Create(1, "File Load failed");

            _sourceInfo = Bass.ChannelGetInfo(_sourceHandle);
            _mixerHandle = BassMix.CreateMixerStream(_sourceInfo.Frequency, _sourceInfo.Channels, mixerflags);

            if (_mixerHandle == 0)
                ExceptionFactory.Create(2, "Mixer failed");

            if (!BassMix.MixerAddChannel(_mixerHandle, _sourceHandle, BassFlags.MixerDownMix))
                ExceptionFactory.Create(3, "Channel mixing failed");

            Bass.ChannelSetAttribute(_mixerHandle, ChannelAttribute.Volume, _lastvol);
            InitEq(ref _mixerHandle);
            Bass.ChannelPlay(_mixerHandle, false);
            _paused = false;
            IsPlaying = true;
        }

        public void SetDevice(int DeviceId)
        {
            throw new NotImplementedException();
        }
    }
}
