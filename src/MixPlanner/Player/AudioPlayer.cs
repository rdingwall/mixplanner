using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MixPlanner.IO.Loader;
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
        static readonly ILog Log = LogManager.GetLogger(typeof (AudioPlayer));
        readonly IDispatcherMessenger messenger;
        readonly NAudioEngine naudio;

        public Track CurrentTrack { get; private set; }

        public AudioPlayer(IDispatcherMessenger messenger, NAudioEngine naudio)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (naudio == null) throw new ArgumentNullException("naudio");
            this.messenger = messenger;
            this.naudio = naudio;

            naudio.Stopped += OnStopped;
            naudio.Started += OnStarted;
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
                    naudio.OpenFile(track.Filename);
                }
                catch (Exception e)
                {
                    Log.WarnFormat("Unable to play {0}.", track.Filename);
                    Log.Warn(e);
                    return;
                }

                CurrentTrack = track;
                messenger.SendToUI(new PlayerLoadedEvent(track));
            }

            naudio.Play();
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
                && naudio.IsPlaying;
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
            naudio.Pause();
        }

        public async Task StopAsync()
        {
            await Task.Run(() => Stop());
        }

        void Stop()
        {
            if (!HasTrackLoaded())
                return;

            naudio.Stop();

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

                    naudio.Started -= OnStarted;
                    naudio.Stopped -= OnStopped;
                }

                disposed = true;
            }
        }

        void OnStarted(object sender, EventArgs e)
        {
            messenger.SendToUI(new PlayerPlayingEvent(CurrentTrack));
        }

        void OnStopped(object sender, EventArgs e)
        {
            messenger.SendToUI(new PlayerStoppedEvent(CurrentTrack));
        }

        ~AudioPlayer()
        {
            Dispose(false);
        }

        bool disposed;
    }
}