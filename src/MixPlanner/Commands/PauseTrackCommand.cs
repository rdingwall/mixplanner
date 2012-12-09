using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PauseTrackCommand : CommandBase<Track>
    {
        readonly IAudioPlayer player;

        public PauseTrackCommand(IAudioPlayer player, IMessenger messenger)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.player = player;
            messenger.Register<PlayerStoppedEvent>(this, _ => RaiseCanExecuteChanged());
            messenger.Register<PlayerPlayingEvent>(this, _ => RaiseCanExecuteChanged());
        }

        protected override bool CanExecute(Track parameter)
        {
            if (parameter == null)
                return false;

            return player.IsPlaying(parameter);
        }

        protected override void Execute(Track parameter)
        {
            player.Pause();
        }
    }
}