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

        protected override bool DoCanExecute(Track parameter)
        {
            return parameter != null && player.CanPlay(parameter);
        }

        protected override void DoExecute(Track parameter)
        {
            player.PlayOrResume(parameter);
        }
    }
}