using System;
using System.Windows;
using System.Windows.Input;
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
        public MixItem MixItem { get; private set; }
        public HarmonicKey ActualKey { get { return MixItem.PlaybackSpeed.ActualKey; } }
        public double ActualBpm { get { return MixItem.PlaybackSpeed.ActualBpm; } }

        public double PlaySpeed
        {
            get { return MixItem.PlaybackSpeed.Speed; }
            set { mix.AdjustPlaybackSpeed(MixItem, value); }
        }

        public Track Track { get { return MixItem.Track; }}
        public string Title { get { return MixItem.Track.Title;} }
        public string Artist { get { return MixItem.Track.Artist;} }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public string Transition { get { return MixItem.Transition.Description; } }
        public bool IsSelected { get; set; }

        public MixItemViewModel(
            IMessenger messenger, 
            MixItem mixItem,
            PlayPauseTrackCommand playPauseCommand,
            IMix mix) : base(messenger)
        {
            this.mix = mix;
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixItem == null) throw new ArgumentNullException("mixItem");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            if (mix == null) throw new ArgumentNullException("mix");
            MixItem = mixItem;
            PlayPauseCommand = playPauseCommand;
            messenger.Register<TransitionChangedEvent>(this, OnTransitionChanged);
            messenger.Register<PlaybackSpeedAdjustedEvent>(this, OnPlaybackSpeedAdjusted);

            // Required for play/pause status
            messenger.Register<PlayerPlayingEvent>(this, _ => RaisePropertyChanged(() => Track));
            messenger.Register<PlayerStoppedEvent>(this, _ => RaisePropertyChanged(() => Track));
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
            dragInfo.Data = this;
            dragInfo.Effects = DragDropEffects.Move | DragDropEffects.Copy;
        }

        public void Dropped(IDropInfo dropInfo)
        {
            
        }
    }
}