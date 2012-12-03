using System;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayTrackCommand : CommandBase<Track>
    {
        readonly IAudioPlayer player;

        public PlayTrackCommand(IAudioPlayer player)
        {
            if (player == null) throw new ArgumentNullException("player");
            this.player = player;
        }

        protected override bool CanExecute(Track parameter)
        {
            return parameter != null;
        }

        protected override void Execute(Track parameter)
        {
            player.Play(parameter.Filename);
        }
    }
}