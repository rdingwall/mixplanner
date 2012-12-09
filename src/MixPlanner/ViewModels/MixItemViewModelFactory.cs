using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Player;

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
        readonly IAudioPlayer player;

        public MixItemViewModelFactory(
            IMessenger messenger, 
            IMix mix, 
            IAudioPlayer player)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mix == null) throw new ArgumentNullException("mix");
            if (player == null) throw new ArgumentNullException("player");
            this.messenger = messenger;
            this.mix = mix;
            this.player = player;
        }

        public MixItemViewModel CreateFor(MixItem item)
        {
            var command = new PlayOrPauseTrackCommand(player, messenger, item.Track);
            return new MixItemViewModel(messenger, item, command, mix);
        }
    }
}