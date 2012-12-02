using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixItemViewModel : INotifyPropertyChanged
    {
        public MixItem MixItem { get; private set; }
        public HarmonicKey Key { get { return MixItem.Track.Key; } }

        public MixItemViewModel(
            IMessenger messenger, 
            MixItem mixItem)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixItem == null) throw new ArgumentNullException("mixItem");
            MixItem = mixItem;
            messenger.Register<TransitionChangedEvent>(this, OnTransitionChanged);
        }

        void OnTransitionChanged(TransitionChangedEvent obj)
        {
            if (obj.MixItem != MixItem)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs("Text"));
        }

        public string Text
        {
            get
            {
                return string.Format("{0}{1}{2} {3}",
                                     MixItem.Transition.Description,
                                     Environment.NewLine,
                                     MixItem.Track.Key, MixItem.Track.Title);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}