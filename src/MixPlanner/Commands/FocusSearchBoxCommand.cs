using System;
using System.Windows.Input;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class FocusSearchBoxCommand : ICommand
    {
        readonly IDispatcherMessenger messenger;

        public FocusSearchBoxCommand(IDispatcherMessenger messenger)
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
            messenger.SendToUI(new SearchBoxFocusRequestedEvent());
        }

        public event EventHandler CanExecuteChanged;
    }
}