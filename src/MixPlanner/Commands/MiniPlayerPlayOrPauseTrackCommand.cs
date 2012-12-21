using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class MiniPlayerPlayOrPauseTrackCommand : ICommand
    {
        readonly IAudioPlayer player;

        public MiniPlayerPlayOrPauseTrackCommand(
            IAudioPlayer player,
            IMessenger messenger)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.player = player;
            messenger.Register<PlayerPlayingEvent>(this, OnPlayerPlaying);
        }

        void OnPlayerPlaying(PlayerPlayingEvent obj)
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return player.HasTrackLoaded();
        }

        public void Execute(object parameter)
        {
            if (player.IsPlaying())
                player.Pause();
            else
                player.PlayOrResume();
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}