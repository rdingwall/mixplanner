using System;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class AudioPlayerViewModel : ViewModelBase
    {
        bool hasTrackLoaded;
        Track track;
        string title;
        string artist;
        TimeSpan duration;
        ImageSource imageSource;
        HarmonicKey originalKey;
        double originalBpm;
        public OpenSettingsCommand OpenSettingsCommand { get; private set; }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public PlayPreviousTrackCommand PlayPreviousCommand { get; private set; }
        public PlayNextTrackCommand PlayNextCommand { get; private set; }

        public AudioPlayerViewModel(
            IMessenger messenger,
            OpenSettingsCommand openSettingsCommand,
            PlayPauseTrackCommand playPauseCommand,
            PlayPreviousTrackCommand playPreviousCommand,
            PlayNextTrackCommand playNextCommand)
            : base(messenger)
        {
            if (openSettingsCommand == null) throw new ArgumentNullException("openSettingsCommand");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            if (playPreviousCommand == null) throw new ArgumentNullException("playPreviousCommand");
            if (playNextCommand == null) throw new ArgumentNullException("playNextCommand");
            OpenSettingsCommand = openSettingsCommand;
            PlayPauseCommand = playPauseCommand;
            PlayPreviousCommand = playPreviousCommand;
            PlayNextCommand = playNextCommand;

            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerLoadedEvent>(this, OnLoaded);
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
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

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                RaisePropertyChanged(() => Artist);
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

        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RaisePropertyChanged(() => Duration);
            }
        }

        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                RaisePropertyChanged(() => ImageSource);
            }
        }

        public HarmonicKey OriginalKey
        {
            get { return originalKey; }
            set
            {
                originalKey = value;
                RaisePropertyChanged(() => OriginalKey);
            }
        }

        public double OriginalBpm
        {
            get { return originalBpm; }
            set
            {
                originalBpm = value;
                RaisePropertyChanged(() => OriginalBpm);
            }
        }

        void OnLoaded(PlayerLoadedEvent obj)
        {
            HasTrackLoaded = true;
            Track = obj.Track;
            Title = obj.Track.Title;
            Artist = obj.Track.Artist;
            ImageSource = obj.Track.Get64x64ImageSource();
            OriginalBpm = obj.Track.OriginalBpm;
            OriginalKey = obj.Track.OriginalKey;
            Duration = obj.Track.Duration;
        }
    }
}