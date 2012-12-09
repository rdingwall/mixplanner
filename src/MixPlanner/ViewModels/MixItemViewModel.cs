using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixItemViewModel : ViewModelBase
    {
        public MixItem MixItem { get; private set; }
        public HarmonicKey ActualKey { get { return MixItem.PlaybackSpeed.ActualKey; } }
        public double ActualBpm { get { return MixItem.PlaybackSpeed.ActualBpm; } }
        public double PlaySpeed { get { return MixItem.PlaybackSpeed.PercentIncrease; } }
        public Track Track { get { return MixItem.Track; }}
        public string Title { get { return MixItem.Track.Title;} }
        public string Artist { get { return MixItem.Track.Artist;} }
        public PlayOrPauseTrackCommand PlayPauseCommand { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool IsNotPlaying { get { return !IsPlaying; } }

        public MixItemViewModel(
            IMessenger messenger, 
            MixItem mixItem,
            PlayOrPauseTrackCommand playPauseCommand) : base(messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixItem == null) throw new ArgumentNullException("mixItem");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            MixItem = mixItem;
            PlayPauseCommand = playPauseCommand;
            messenger.Register<TransitionChangedEvent>(this, OnTransitionChanged);
        }

        void OnTransitionChanged(TransitionChangedEvent obj)
        {
            if (obj.MixItem != MixItem)
                return;

            RaisePropertyChanged(() => Text);
        }

        public string Text
        {
            get
            {
                return string.Format("{0}{1}{2} {3}",
                                     MixItem.Transition.Description,
                                     Environment.NewLine,
                                     MixItem.Track.OriginalKey, MixItem.Track.Title);
            }
        }
    }
}