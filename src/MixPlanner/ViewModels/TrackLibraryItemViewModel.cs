using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryItemViewModel : ViewModelBase
    {
        Transition transition;

        public TrackLibraryItemViewModel(IMessenger messenger, Track track)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            Artist = track.Artist;
            Title = track.Title;
            Genre = track.Genre;
            Bpm = track.OriginalBpm;
            Year = track.Year;
            Label = track.Label;
            Filename = track.File.FullName;
            Key = track.OriginalKey;

            messenger.Register<RecommendationsClearedEvent>(this, _ => Transition = null);
            messenger.Register<TrackRecommendedEvent>(this, OnTrackRecommended);

            // Required for play/pause status
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<ConfigSavedEvent>(this, _ => RaisePropertyChanged(() => Key));
        }

        void OnTrackRecommended(TrackRecommendedEvent obj)
        {
            if (obj.Track != Track)
                return;

            Transition = obj.Transition;
        }

        public string TransitionDescription
        {
            get { return Transition != null ? Transition.Description : null; }
        }

        public Transition Transition
        {
            get { return transition; }
            set
            {
                transition = value;
                RaisePropertyChanged(() => Transition);
                RaisePropertyChanged(() => TransitionDescription);
                RaisePropertyChanged(() => IsCompatible);
            }
        }

        public bool IsCompatible
        {
            get { return transition != null; }
        }

        public string Filename { get; set; }

        public string Artist { get; set; }
        public string Title { get; set; }

        public HarmonicKey Key { get; set; }

        public string Genre { get; set; }
        public string Label { get; set; }
        public string Year { get; set; }
        public double Bpm { get; set; }

        public Track Track { get; set; }
        public bool IsSelected { get; set; }
    }
}