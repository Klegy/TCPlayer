using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TCPlayer.Engine
{
    public partial class Engine : IEngine
    {
        public int CurrentDeviceID => throw new NotImplementedException();

        public EqualizerConfig Equalizer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsPaused => throw new NotImplementedException();

        public MediaKind CurrentMediaKind => throw new NotImplementedException();

        public double Length => throw new NotImplementedException();

        public double Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool GetChannelData(out short[] data, float seconds)
        {
            throw new NotImplementedException();
        }

        public bool GetFFTData(float[] fftDataBuffer)
        {
            throw new NotImplementedException();
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            throw new NotImplementedException();
        }

        public void Load(string url)
        {
            throw new NotImplementedException();
        }

        public void PlayPause()
        {
            throw new NotImplementedException();
        }

        public void SetDevice(int DeviceId)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
