using System;
using System.ComponentModel;

namespace TCPlayer.Engine
{
    public interface IEngine: IDisposable, INotifyPropertyChanged, ISpectrumProvider
    {
        int CurrentDeviceID { get; }
        EqualizerConfig Equalizer { get; set; }
        bool IsPaused { get; }
        MediaKind CurrentMediaKind { get; }
        double Length { get; }
        double Position { get; set; }
        float Volume { get; set; }
        void SetDevice(int DeviceId);
        void Load(string url);
        void PlayPause();
        void Stop();
    }
}
