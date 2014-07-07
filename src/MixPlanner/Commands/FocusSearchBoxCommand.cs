namespace MixPlanner.Commands
{
    using System;
    using System.Windows.Input;
    using MixPlanner.Events;

    public sealed class FocusSearchBoxCommand : ICommand
    {
        private readonly IDispatcherMessenger messenger;

        public FocusSearchBoxCommand(IDispatcherMessenger messenger)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }

            this.messenger = messenger;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            messenger.SendToUI(new SearchBoxFocusRequestedEvent());
        }
    }
}