using System;
using System.Windows;
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
            get { return MixItem.PlaybackSpeed.PercentIncrease; }
            set { mix.AdjustPlaybackSpeed(MixItem, value); }
        }

        public Track Track { get { return MixItem.Track; }}
        public string Title { get { return MixItem.Track.Title;} }
        public string Artist { get { return MixItem.Track.Artist;} }
        public PlayOrPauseTrackCommand PlayPauseCommand { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool IsNotPlaying { get { return !IsPlaying; } }
        public string Transition { get { return MixItem.Transition.Description; } }

        public MixItemViewModel(
            IMessenger messenger, 
            MixItem mixItem,
            PlayOrPauseTrackCommand playPauseCommand,
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

        public void StartDrag(DragInfo dragInfo)
        {
            dragInfo.Data = this;
            dragInfo.Effects = DragDropEffects.Move | DragDropEffects.Copy;
        }
    }
}