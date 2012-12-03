using System;
using System.Windows.Input;

namespace MixPlanner.Commands
{
    public abstract class CommandBase<T> : ICommand
    {
        public bool CanExecute(object parameter)
        {
            if (!(parameter is T))
                return false;

            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (!(parameter is T))
                return;

            Execute((T)parameter);
        }

        protected virtual bool CanExecute(T parameter)
        {
            return true;
        }

        protected abstract void Execute(T parameter);

        public event EventHandler CanExecuteChanged = delegate { };
    }
}