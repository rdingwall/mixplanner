namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight.Messaging;
    using MixPlanner.DomainModel;
    using MixPlanner.Events;
    using MixPlanner.Player;

    public abstract class PlaylistCommandBase : AsyncCommandBase
    {
        private readonly IPlaylist playlist;
        private readonly IAudioPlayer player;
        private readonly Func<IPlaylist, Track> trackSelectFunc;

        protected PlaylistCommandBase(
            IPlaylist playlist, IMessenger messenger, IAudioPlayer player, Func<IPlaylist, Track> trackSelectFunc)
        {
            if (playlist == null)
            {
                throw new ArgumentNullException("playlist");
            }

            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }

            if (player == null)
            {
                throw new ArgumentNullException("player");
            }

            if (trackSelectFunc == null)
            {
                throw new ArgumentNullException("trackSelectFunc");
            }

            this.playlist = playlist;
            this.player = player;
            this.trackSelectFunc = trackSelectFunc;

            // Need to refresh after adding, removing, shuffling any tracks in
            // the mix. The prev/next track may no longer exist.
            messenger.Register<PlayerLoadedEvent>(this, OnMixChanged);
            messenger.Register<TrackAddedToMixEvent>(this, OnMixChanged);
            messenger.Register<TrackRemovedFromMixEvent>(this, OnMixChanged);
            messenger.Register<TracksRemovedFromMixEvent>(this, OnMixChanged);
            messenger.Register<MixUnlockedEvent>(this, OnMixChanged);
        }

        public override bool CanExecute(object parameter)
        {
            Track track = trackSelectFunc(playlist);
            return track != null && player.CanPlay(track);
        }

        protected override async Task DoExecute(object parameter)
        {
            Track track = trackSelectFunc(playlist);
            await player.PlayOrResumeAsync(track);
        }

        private void OnMixChanged(object obj)
        {
            RaiseCanExecuteChanged();
        }
    }
}