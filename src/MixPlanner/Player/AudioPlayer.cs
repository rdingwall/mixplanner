using System;
using NAudio.Wave;

namespace MixPlanner.Player
{
    public interface IAudioPlayer : IDisposable
    {
        void Play(string filename);
        void PauseOrResume();
        void Stop();
    }

    public class AudioPlayer : IAudioPlayer
    {
        public AudioPlayer()
        {
            waveOutDevice = new WaveOut();
        }

        public void Play(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            Stop();
            stream = new Mp3FileReader(filename);
            waveOutDevice.Init(stream);
            waveOutDevice.Play();
        }

        public void PauseOrResume()
        {
            if (waveOutDevice.PlaybackState == PlaybackState.Paused)
                waveOutDevice.Resume();
            else
                waveOutDevice.Pause();
        }

        public void Stop()
        {
            waveOutDevice.Stop();
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Stop();

                    if (waveOutDevice != null)
                    {
                        waveOutDevice.Dispose();
                        waveOutDevice = null;
                    }
                }

                disposed = true;
            }
        }

        ~AudioPlayer()
        {
            Dispose(false);
        }

        bool disposed;
        WaveOut waveOutDevice;
        Mp3FileReader stream;
    }
}