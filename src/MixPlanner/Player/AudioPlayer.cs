using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using NAudio.Wave;

namespace MixPlanner.Player
{
    public interface IAudioPlayer
    {
        Track CurrentTrack { get; }
        bool CanPlay(Track track);
        void PlayOrResume(Track track);

        bool IsPlaying(Track track);
        void Pause();
        void Stop();
    }

    public class AudioPlayer : IAudioPlayer, IDisposable
    {
        readonly IMessenger messenger;
        public Track CurrentTrack { get; private set; }

        public AudioPlayer(IMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
            waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += delegate { NotifyStopped(); };
        }

        void NotifyStopped()
        {
            messenger.Send(new PlayerStoppedEvent(CurrentTrack));
        }

        void NotifyStarting()
        {
            messenger.Send(new PlayerPlayingEvent(CurrentTrack));
        }

        public bool CanPlay(Track track)
        {
            return track != null && !String.IsNullOrWhiteSpace(track.Filename);
        }

        public void PlayOrResume(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            if (!CanPlay(track)) return;

            if (!track.Equals(CurrentTrack))
            {
                Stop();
                CurrentTrack = track;
                stream = new Mp3FileReader(track.Filename);
                waveOutDevice.Init(stream);
            }

            waveOutDevice.Play();
            NotifyStarting();
        }

        public bool IsPlaying(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return track.Equals(CurrentTrack) 
                && waveOutDevice.PlaybackState == PlaybackState.Playing;
        }

        public void Pause()
        {
            if (waveOutDevice.PlaybackState != PlaybackState.Playing)
                return;

            waveOutDevice.Pause();
            NotifyStopped();
        }

        public void Stop()
        {
            if (waveOutDevice.PlaybackState == PlaybackState.Stopped)
                return;

            if (CurrentTrack == null)
                return;

            waveOutDevice.Stop();
            CurrentTrack = null;
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }

            NotifyStopped();
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