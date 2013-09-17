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

        public OpenMixCommand OpenCommand { get; private set; }
        public SaveMixCommand SaveCommand { get; private set; }
        public SaveMixAsCommand SaveAsCommand { get; private set; }

        public MixToolBarViewModel(
            OpenMixCommand openCommand,
            SaveMixCommand saveCommand,
            SaveMixAsCommand saveAsCommand,
            IMessenger messenger) : base(messenger)
        {
            if (openCommand == null) throw new ArgumentNullException("openCommand");
            if (saveCommand == null) throw new ArgumentNullException("saveCommand");
            if (saveAsCommand == null) throw new ArgumentNullException("saveAsCommand");
            OpenCommand = openCommand;
            SaveCommand = saveCommand;
            SaveAsCommand = saveAsCommand;
            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
        }

        void OnMixLoaded(MixLoadedEvent obj)
        {
            Mix = obj.Mix;
        }
    }
}