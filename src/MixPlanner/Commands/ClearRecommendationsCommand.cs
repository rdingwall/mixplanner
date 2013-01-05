using System;
using System.Windows.Input;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class ClearRecommendationsCommand : ICommand
    {
        readonly IDispatcherMessenger messenger;

        public ClearRecommendationsCommand(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            messenger.SendToUI(new RecommendationsClearedEvent());
        }

        public event EventHandler CanExecuteChanged = delegate {};
    }
}