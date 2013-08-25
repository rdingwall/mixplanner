using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MixPlanner.Player;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryItemViewModel : ViewModelBase
    {
        readonly IAudioPlayer player;
        Transition transition;
        string artist;
        string title;
        HarmonicKey key;
        string genre;
        string label;
        string year;
        double bpm;
        string filename;
        bool isCorrespondingMixItemSelected;

        public TrackLibraryItemViewModel(
            IMessenger messenger, 
            IAudioPlayer player,
            Track track,
            QuickEditBpmCommand quickEditBpmCommand,
            QuickEditHarmonicKeyCommand quickEditHarmonicKeyCommand,
            PlayPauseTrackCommand playPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (track == null) throw new ArgumentNullException("track");
            if (quickEditBpmCommand == null) throw new ArgumentNullException("quickEditBpmCommand");
            if (quickEditHarmonicKeyCommand == null) throw new ArgumentNullException("quickEditHarmonicKeyCommand");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            this.player = player;
            Track = track;
            QuickEditBpmCommand = quickEditBpmCommand;
            QuickEditHarmonicKeyCommand = quickEditHarmonicKeyCommand;
            PlayPauseCommand = playPauseCommand;
            PopulateFields(track);

            messenger.Register<RecommendationsClearedEvent>(this, _ => Transition = null);
            messenger.Register<TrackRecommendedEvent>(this, OnTrackRecommended);
            messenger.Register<TrackUpdatedEvent>(this, OnTrackUpdated);
            messenger.Register<MixItemSelectionChangedEvent>(this, OnTracksSelected);

            // Required for play/pause status
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<ConfigSavedEvent>(this, _ => RaisePropertyChanged(() => Key));
        }

        void OnTracksSelected(MixItemSelectionChangedEvent obj)
        {
            IsCorrespondingMixItemSelected = obj.SelectedItems.Select(m => m.Track).Contains(Track);
        }

        public bool IsCorrespondingMixItemSelected
        {
            get { return isCorrespondingMixItemSelected; }
            private set
            {
                isCorrespondingMixItemSelected = value;
                RaisePropertyChanged(() => IsCorrespondingMixItemSelected);
                RaisePropertyChanged(() => IsNeitherCompatibleNorSelected);
            }
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
            RaisePropertyChanged(() => ImageSource);
            SearchIndexData = String.Concat(Track.Artist,
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
                RaisePropertyChanged(() => IsNeitherCompatibleNorSelected);
            }
        }

        public bool IsCompatible
        {
            get { return transition != null; }
        }

        public bool IsNeitherCompatibleNorSelected
        {
            get { return !IsCompatible && !IsCorrespondingMixItemSelected; }
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
                RaisePropertyChanged(() => HasKey);
                RaisePropertyChanged(() => IsMissingKey);
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
                RaisePropertyChanged(() => Bpm);
                RaisePropertyChanged(() => HasBpm);
                RaisePropertyChanged(() => IsMissingBpm);
            }
        }

        public bool HasBpm
        {
            get { return !Double.IsNaN(bpm); }
        }

        public bool IsMissingBpm
        {
            get { return !HasBpm; }
        }

        public bool HasKey
        {
            get { return !Key.IsUnknown; }
        }

        public bool IsMissingKey
        {
            get { return !HasKey; }
        }

        public ImageSource ImageSource
        {
            get { return Track.Get24x24ImageSource(); }
        }

        public string SearchIndexData { get; private set; }

        public bool IsPlaying { get { return player.IsPlaying(Track); } }

        public Track Track { get; private set; }
        public QuickEditBpmCommand QuickEditBpmCommand { get; private set; }
        public QuickEditHarmonicKeyCommand QuickEditHarmonicKeyCommand { get; private set; }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public bool IsSelected { get; set; }
    }
}