using System;
using System.Windows.Input;
using log4net;

namespace MixPlanner.Commands
{
    public abstract class CommandBase<T> : ICommand
    {
        protected readonly ILog Log;

        protected CommandBase()
        {
            Log = LogManager.GetLogger(GetType());
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is T))
            {
                LogWrongParameterMessage(parameter);
                return false;
            }

            return DoCanExecute((T)parameter);
        }

        void LogWrongParameterMessage(object parameter)
        {
            Log.ErrorFormat("Invalid command parameter received, excepted {0} but got '{1}'",
                            typeof(T), parameter);
        }

        public void Execute(object parameter)
        {
            if (!(parameter is T))
            {
                LogWrongParameterMessage(parameter);
                return;
            }

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