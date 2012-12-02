using System;
using System.Windows.Input;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayTrackCommand : ICommand
    {
        readonly IAudioPlayer player;

        public PlayTrackCommand(IAudioPlayer player)
        {
            if (player == null) throw new ArgumentNullException("player");
            this.player = player;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var track = (Track) parameter;

            player.Play(track.Filename);
        }

        public event EventHandler CanExecuteChanged;
    }
}