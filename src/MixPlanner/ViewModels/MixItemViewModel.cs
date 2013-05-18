using System;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixItemViewModel : ViewModelBase, IDragSource
    {
        readonly IMix mix;
        readonly MixViewModel mixViewModel;
        bool isLocked;
        public IMixItem MixItem { get; private set; }
        public HarmonicKey ActualKey { get { return MixItem.PlaybackSpeed.ActualKey; } }
        public double ActualBpm { get { return MixItem.PlaybackSpeed.ActualBpm; } }

        public double PlaySpeed
        {
            get { return MixItem.PlaybackSpeed.Speed; }
            set { mix.AdjustPlaybackSpeed(MixItem, value); }
        }

        public double PitchFaderStepSize { get { return PitchFaderStep.Value; } }
        public double PitchFaderMax { get { return 1 + (PitchFaderStep.Value * PitchFaderStep.NumberOfSteps); } }
        public double PitchFaderMin { get { return 1 - (PitchFaderStep.Value * PitchFaderStep.NumberOfSteps); } }

        public Track Track { get { return MixItem.Track; }}
        public string Title { get { return MixItem.Track.Title;} }
        public string Artist { get { return MixItem.Track.Artist;} }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public Transition Transition { get { return MixItem.Transition; } }
        public bool IsSelected { get; set; }

        public bool IsPitchFaderEnabled
        {
            get
            {
                return !mix.IsLocked && !MixItem.PlaybackSpeed.IsUnknownBpm;
            }
        }

        public MixItemViewModel(
            IMessenger messenger, 
            IMixItem mixItem,
            PlayPauseTrackCommand playPauseCommand,
            IMix mix,
            MixViewModel mixViewModel) : base(messenger)
        {
            this.mix = mix;
            this.mixViewModel = mixViewModel;
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixItem == null) throw new ArgumentNullException("mixItem");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            if (mix == null) throw new ArgumentNullException("mix");
            if (mixViewModel == null) throw new ArgumentNullException("mixViewModel");
            MixItem = mixItem;
            PlayPauseCommand = playPauseCommand;
            messenger.Register<TransitionChangedEvent>(this, OnTransitionChanged);
            messenger.Register<PlaybackSpeedAdjustedEvent>(this, OnPlaybackSpeedAdjusted);
            messenger.Register<TrackUpdatedEvent>(this, OnTrackUpdated);

            messenger.Register<MixLockedEvent>(this, _ => RaisePropertyChanged(() => IsPitchFaderEnabled));
            messenger.Register<MixUnlockedEvent>(this, _ => RaisePropertyChanged(() => IsPitchFaderEnabled));

            // Required for play/pause status
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<ConfigSavedEvent>(this, _ => RaisePropertyChanged(() => ActualKey));
        }

        void OnTrackUpdated(TrackUpdatedEvent obj)
        {
            if (!obj.Track.Equals(Track))
                return;

            RaisePropertyChanged(() => Artist);
            RaisePropertyChanged(() => Title);
            RaisePropertyChanged(() => ActualBpm);
            RaisePropertyChanged(() => ActualKey);
            RaisePropertyChanged(() => PlaySpeed);
            RaisePropertyChanged(() => ImageSource);
            RaisePropertyChanged(() => IsPitchFaderEnabled);
        }

        void OnPlaybackSpeedAdjusted(PlaybackSpeedAdjustedEvent obj)
        {
            if (obj.MixItem != MixItem)
                return;

            RaisePropertyChanged(() => ActualBpm);
            RaisePropertyChanged(() => ActualKey);
            RaisePropertyChanged(() => PlaySpeed);
        }

        void OnTransitionChanged(TransitionChangedEvent obj)
        {
            if (obj.MixItem != MixItem)
                return;

            RaisePropertyChanged(() => Transition);
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            if (isLocked)
                return;

            dragInfo.Data = mixViewModel.SelectedItems;
            dragInfo.Effects = DragDropEffects.Move | DragDropEffects.Copy;
        }

        public void Dropped(IDropInfo dropInfo)
        {
            
        }

        public ImageSource ImageSource
        {
            get { return Track.Get64x64ImageSource(); }
        }
    }
}