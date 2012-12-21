using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MiniPlayerViewModel : ViewModelBase
    {
        public ICommand PlayOrPauseCommand { get; private set; }
        string title;
        string playOrPauseButtonLabel;
        bool hasTrackLoaded;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string PlayOrPauseButtonLabel
        {
            get { return playOrPauseButtonLabel; }
            set
            {
                playOrPauseButtonLabel = value;
                RaisePropertyChanged(() => PlayOrPauseButtonLabel);
            }
        }

        public bool HasTrackLoaded
        {
            get { return hasTrackLoaded; }
            set
            {
                hasTrackLoaded = value;
                RaisePropertyChanged(() => HasTrackLoaded);
            }
        }

        public MiniPlayerViewModel(
            IMessenger messenger,
            MiniPlayerPlayOrPauseTrackCommand playOrPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (playOrPauseCommand == null) throw new ArgumentNullException("playOrPauseCommand");
            PlayOrPauseCommand = playOrPauseCommand;

            messenger.Register<PlayerStoppedEvent>(this, OnPlayerStopped);
            messenger.Register<PlayerPlayingEvent>(this, OnPlayerPlaying);
        }

        void OnPlayerPlaying(PlayerPlayingEvent obj)
        {
            PlayOrPauseButtonLabel = "Pause";
            HasTrackLoaded = true;
            Title = String.Format("{0} - {1}", obj.Track.Artist, obj.Track.Title);
        }

        void OnPlayerStopped(PlayerStoppedEvent obj)
        {
            PlayOrPauseButtonLabel = "Play";
        }
    }
}