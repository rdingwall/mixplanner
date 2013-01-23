﻿using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using NAudio.Wave;

namespace MixPlanner.Player
{
    public interface IAudioPlayer
    {
        Track CurrentTrack { get; }
        bool CanPlay(Track track);
        Task PlayOrResumeAsync(Track track);
        Task PlayOrResumeAsync();
        bool IsPlaying(Track track);
        bool IsPlaying();
        Task PauseAsync();
        Task StopAsync();
        bool HasTrackLoaded();
    }

    public class AudioPlayer : IAudioPlayer, IDisposable
    {
        readonly IDispatcherMessenger messenger;
        WaveOut waveOutDevice;
        WaveStream stream;

        public Track CurrentTrack { get; private set; }

        public AudioPlayer(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
            waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += delegate { NotifyStopped(); };
        }

        void NotifyStopped()
        {
            messenger.SendToUI(new PlayerStoppedEvent(CurrentTrack));
        }

        void NotifyStarting()
        {
            messenger.SendToUI(new PlayerPlayingEvent(CurrentTrack));
        }

        public bool HasTrackLoaded()
        {
            return CurrentTrack != null;
        }

        public bool CanPlay(Track track)
        {
            return track != null && !String.IsNullOrWhiteSpace(track.Filename);
        }

        public async Task PlayOrResumeAsync(Track track)
        {
            await Task.Run(() => PlayOrResume(track));
        }

        void PlayOrResume(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            if (!CanPlay(track)) return;

            if (!track.Equals(CurrentTrack))
            {
                Stop();
                CurrentTrack = track;
                stream = new AudioFileReader(track.Filename);
                
                waveOutDevice.Init(stream);
            }

            waveOutDevice.Play();
            NotifyStarting();
        }

        public async Task PlayOrResumeAsync()
        {
            await Task.Run(() => PlayOrResume());
        }

        void PlayOrResume()
        {
            if (!HasTrackLoaded())
                return;

            PlayOrResume(CurrentTrack);
        }

        public bool IsPlaying(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return track.Equals(CurrentTrack) 
                && waveOutDevice.PlaybackState == PlaybackState.Playing;
        }

        public bool IsPlaying()
        {
            return HasTrackLoaded() && IsPlaying(CurrentTrack);
        }

        public async Task PauseAsync()
        {
            await Task.Run(() => Pause());
        }

        void Pause()
        {
            if (waveOutDevice.PlaybackState != PlaybackState.Playing)
                return;

            waveOutDevice.Pause();
            NotifyStopped();
        }

        public async Task StopAsync()
        {
            await Task.Run(() => Stop());
        }

        void Stop()
        {
            if (waveOutDevice.PlaybackState == PlaybackState.Stopped)
                return;

            if (!HasTrackLoaded())
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
    }
}