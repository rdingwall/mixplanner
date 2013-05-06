using System;
using System.Windows.Input;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public abstract class RequestFocusCommandBase<T> : ICommand
    {
        readonly IDispatcherMessenger messenger;

        protected RequestFocusCommandBase(IDispatcherMessenger messenger)
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
            messenger.SendToUI(new FocusRequestedEvent<T>());
        }

        public event EventHandler CanExecuteChanged;
    }
}