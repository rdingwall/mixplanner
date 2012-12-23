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

            return CanExecute((T)parameter);
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

            Execute((T)parameter);
        }

        protected virtual bool CanExecute(T parameter)
        {
            return true;
        }

        protected abstract void Execute(T parameter);

        // CanExecuteChanged whenever a property changes
        // http://stackoverflow.com/a/3092873/91551
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}