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

            return DoCanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (!(parameter is T))
                return;

            DoExecute((T)parameter);
        }

        protected virtual bool DoCanExecute(T parameter)
        {
            return true;
        }

        protected abstract void DoExecute(T parameter);

        protected void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}