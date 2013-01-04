using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayPauseTrackCommand : AsyncCommandBase<Track>
    {
        readonly IAudioPlayer player;

        public PlayPauseTrackCommand(IAudioPlayer player)
        {
            if (player == null) throw new ArgumentNullException("player");
            this.player = player;
        }

        protected override bool CanExecute(Track parameter)
        {
            return player.CanPlay(parameter);
        }

        protected override async Task DoExecute(Track parameter)
        {
            if (player.IsPlaying(parameter))
                await player.PauseAsync();
            else
                await player.PlayOrResumeAsync(parameter);
        }
    }
}