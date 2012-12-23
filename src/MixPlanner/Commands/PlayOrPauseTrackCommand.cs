﻿using System;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayOrPauseTrackCommand : CommandBase<Track>
    {
        readonly IAudioPlayer player;

        public PlayOrPauseTrackCommand(IAudioPlayer player)
        {
            if (player == null) throw new ArgumentNullException("player");
            this.player = player;
        }

        protected override bool CanExecute(Track parameter)
        {
            return player.CanPlay(parameter);
        }

        protected override void Execute(Track parameter)
        {
            if (player.IsPlaying(parameter))
                player.Pause();
            else
                player.PlayOrResume(parameter);
        }
    }
}