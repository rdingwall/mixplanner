using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public interface IMixItemViewModelFactory
    {
        MixItemViewModel CreateFor(MixItem item);
    }

    public class MixItemViewModelFactory : IMixItemViewModelFactory
    {
        readonly IMessenger messenger;
        readonly IMix mix;
        readonly PlayOrPauseTrackCommand playOrPauseCommand;

        public MixItemViewModelFactory(
            IMessenger messenger, 
            IMix mix, 
            PlayOrPauseTrackCommand playOrPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mix == null) throw new ArgumentNullException("mix");
            if (playOrPauseCommand == null) throw new ArgumentNullException("playOrPauseCommand");
            this.messenger = messenger;
            this.mix = mix;
            this.playOrPauseCommand = playOrPauseCommand;
        }

        public MixItemViewModel CreateFor(MixItem item)
        {
            return new MixItemViewModel(messenger, item, playOrPauseCommand, mix);
        }
    }
}