using System;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryItemViewModel : ViewModelBase
    {
        Transition transition;
        string artist;
        string title;
        HarmonicKey key;
        string genre;
        string label;
        string year;
        double bpm;
        string filename;

        public TrackLibraryItemViewModel(IMessenger messenger, Track track)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            PopulateFields(track);

            messenger.Register<RecommendationsClearedEvent>(this, _ => Transition = null);
            messenger.Register<TrackRecommendedEvent>(this, OnTrackRecommended);
            messenger.Register<TrackUpdatedEvent>(this, OnTrackUpdated);

            // Required for play/pause status
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<ConfigSavedEvent>(this, _ => RaisePropertyChanged(() => Key));
        }

        void PopulateFields(Track track)
        {
            Artist = track.Artist;
            Title = track.Title;
            Genre = track.Genre;
            Bpm = track.OriginalBpm;
            Year = track.Year;
            Label = track.Label;
            Filename = track.File.Name;
            Key = track.OriginalKey;
            SearchData = String.Concat(Track.Artist,
                                       Track.Title,
                                       Path.GetFileNameWithoutExtension(Track.Filename),
                                       Track.OriginalBpm.ToString(),
                                       Track.OriginalKey.ToString());
        }

        void OnTrackUpdated(TrackUpdatedEvent obj)
        {
            if (!obj.Track.Equals(Track))
                return;

            PopulateFields(obj.Track);
        }

        void OnTrackRecommended(TrackRecommendedEvent obj)
        {
            if (!obj.Track.Equals(Track))
                return;

            Transition = obj.Transition;
        }

        public double? IncreaseRequired
        {
            get
            {
                if (Transition == null)
                    return null;

                if (Transition.IncreaseRequired == 0)
                    return null;

                return Transition.IncreaseRequired;
            }
        }

        public Transition Transition
        {
            get { return transition; }
            set
            {
                transition = value;
                RaisePropertyChanged(() => Transition);
                RaisePropertyChanged(() => IncreaseRequired);
                RaisePropertyChanged(() => IsCompatible);
            }
        }

        public bool IsCompatible
        {
            get { return transition != null; }
        }

        public string Filename
        {
            get { return filename; }
            private set
            {
                filename = value;
                RaisePropertyChanged(() => Filename);
            }
        }

        public string Artist
        {
            get { return artist; }
            private set
            {
                artist = value;
                RaisePropertyChanged(() => Artist);
            }
        }

        public string Title
        {
            get { return title; }
            private set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public HarmonicKey Key
        {
            get { return key; }
            private set
            {
                key = value;
                RaisePropertyChanged(() => Key);
            }
        }

        public string Genre
        {
            get { return genre; }
            private set
            {
                genre = value;
                RaisePropertyChanged(() => Genre);
            }
        }

        public string Label
        {
            get { return label; }
            private set
            {
                label = value;
                RaisePropertyChanged(() => Label);
            }
        }

        public string Year
        {
            get { return year; }
            private set
            {
                year = value;
                RaisePropertyChanged(() => Year);
            }
        }

        public double Bpm
        {
            get { return bpm; }
            private set
            {
                bpm = value;
                RaisePropertyChanged(() => Year);
            }
        }

        public string SearchData { get; set; }

        public Track Track { get; private set; }
        public bool IsSelected { get; set; }
    }
}