using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public interface IMixItemViewModelFactory
    {
        MixItemViewModel CreateFor(IMixItem item);
    }

    public class MixItemViewModelFactory : IMixItemViewModelFactory
    {
        readonly IMessenger messenger;
        readonly IMix mix;
        readonly PlayPauseTrackCommand playPauseCommand;

        public MixItemViewModelFactory(
            IMessenger messenger, 
            IMix mix, 
            PlayPauseTrackCommand playPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mix == null) throw new ArgumentNullException("mix");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            this.messenger = messenger;
            this.mix = mix;
            this.playPauseCommand = playPauseCommand;
        }

        public MixItemViewModel CreateFor(IMixItem item)
        {
            return new MixItemViewModel(messenger, item, playPauseCommand, mix);
        }
    }
}