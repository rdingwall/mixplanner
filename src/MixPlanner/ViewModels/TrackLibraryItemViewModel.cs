using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryItemViewModel : ViewModelBase
    {
        double recommendationFactor;

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
            recommendationFactor = 0;

            messenger.Register<RecommendationsClearedEvent>(this, _ => RecommendationFactor = 0);
            messenger.Register<TrackRecommendedEvent>(this, OnTrackRecommended);

            // Required for play/pause status
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
        }

        void OnTrackRecommended(TrackRecommendedEvent obj)
        {
            if (obj.Track != Track)
                return;

            RecommendationFactor = obj.RecommendationFactor;
        }

        public double RecommendationFactor
        {
            get { return recommendationFactor; }
            set
            {
                recommendationFactor = value;
                RaisePropertyChanged(() => RecommendationFactor);
                RaisePropertyChanged(() => IsRecommended);
            }
        }

        public bool IsRecommended
        {
            get { return recommendationFactor > 0; }
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