using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayOrPauseTrackCommand : CommandBase<object>, INotifyPropertyChanged
    {
        readonly IAudioPlayer player;
        readonly Track track;
        bool isPlaying;

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsPlaying"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public PlayOrPauseTrackCommand(
            IAudioPlayer player,
            IMessenger messenger,
            Track track)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (track == null) throw new ArgumentNullException("track");
            this.player = player;
            this.track = track;
            IsPlaying = player.IsPlaying(track);
            messenger.Register<PlayerPlayingEvent>(this, OnBegunPlayingTrack);
            messenger.Register<PlayerStoppedEvent>(this, OnPlayerStopped);
        }

        void OnBegunPlayingTrack(PlayerPlayingEvent obj)
        {
            if (obj.Track.Equals(track))
                IsPlaying = true;
        }

        void OnPlayerStopped(PlayerStoppedEvent obj)
        {
            IsPlaying = false;
        }

        protected override bool DoCanExecute(object parameter)
        {
            return player.CanPlay(track);
        }

        protected override void DoExecute(object parameter)
        {
            if (player.IsPlaying(track))
                player.Pause();
            else
                player.PlayOrResume(track);
        }
    }
}