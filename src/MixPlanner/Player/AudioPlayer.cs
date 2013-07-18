using System;
using System.Threading.Tasks;
using System.Windows;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MixPlanner.Loader;
using NAudio.Wave;
using log4net;

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
        static readonly ILog log = LogManager.GetLogger(typeof (AudioPlayer));
        readonly IDispatcherMessenger messenger;

        public Track CurrentTrack { get; private set; }

        public AudioPlayer(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;

            NAudioEngine.Instance.Stopped += delegate { NotifyStopped(); };
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
            var filename = track.Filename;
            if (track == null || String.IsNullOrWhiteSpace(filename))
                return false;

            return FileNameHelper.IsAiff(filename) ||
                   FileNameHelper.IsMp3(filename) ||
                   FileNameHelper.IsWav(filename);

        }

        public async Task PlayOrResumeAsync(Track track)
        {
            PlayOrResume(track);
        }

        void PlayOrResume(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            if (!CanPlay(track)) return;

            if (!track.Equals(CurrentTrack))
            {
                Stop();
                
                try
                {
                    
                    NAudioEngine.Instance.OpenFile(track.Filename);
                }
                catch (Exception e)
                {
                    log.WarnFormat("Unable to play {0}.", track.Filename);
                    log.Warn(e);
                    return;
                }

                CurrentTrack = track;
            }

            NAudioEngine.Instance.Play();
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
                && NAudioEngine.Instance.IsPlaying;
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
            NAudioEngine.Instance.Pause();
        }

        public async Task StopAsync()
        {
            await Task.Run(() => Stop());
        }

        void Stop()
        {
            if (!HasTrackLoaded())
                return;

            NAudioEngine.Instance.Stop();

            CurrentTrack = null;
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