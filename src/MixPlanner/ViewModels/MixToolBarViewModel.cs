using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixToolBarViewModel : ViewModelBase
    {
        IMix mix;

        public IMix Mix
        {
            get { return mix; }
            private set
            {
                mix = value;
                RaisePropertyChanged(() => Mix);
            }
        }

        public SaveMixAsCommand SaveAsCommand { get; private set; }

        public MixToolBarViewModel(SaveMixAsCommand saveAsCommand, IMessenger messenger) : base(messenger)
        {
            if (saveAsCommand == null) throw new ArgumentNullException("saveAsCommand");
            SaveAsCommand = saveAsCommand;
            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
        }

        void OnMixLoaded(MixLoadedEvent obj)
        {
            Mix = obj.Mix;
        }
    }
}