using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public interface IMixItemViewModelFactory
    {
        MixItemViewModel CreateFor(IMix mix, IMixItem item, MixViewModel mixViewModel);
    }

    public class MixItemViewModelFactory : IMixItemViewModelFactory
    {
        readonly IMessenger messenger;
        readonly PlayPauseTrackCommand playPauseCommand;

        public MixItemViewModelFactory(
            IMessenger messenger, 
            PlayPauseTrackCommand playPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            this.messenger = messenger;
            this.playPauseCommand = playPauseCommand;
        }

        public MixItemViewModel CreateFor(IMix mix, IMixItem item, MixViewModel mixViewModel)
        {
            return new MixItemViewModel(messenger, item, playPauseCommand, mix, mixViewModel);
        }
    }
}