using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MiniPlayerViewModel : ViewModelBase
    {
        public ICommand PlayPauseCommand { get; private set; }
        string title;
        bool hasTrackLoaded;
        Track track;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public Track Track
        {
            get { return track; }
            set
            {
                track = value;
                RaisePropertyChanged(() => Track);
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
            PlayPauseTrackCommand playPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            PlayPauseCommand = playPauseCommand;

            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerPlayingEvent>(this, OnPlayerPlaying);
        }

        void OnPlayerPlaying(PlayerPlayingEvent obj)
        {
            HasTrackLoaded = true;
            Track = obj.Track;
            Title = String.Format("{0} - {1}", obj.Track.Artist, obj.Track.Title);
        }
    }
}