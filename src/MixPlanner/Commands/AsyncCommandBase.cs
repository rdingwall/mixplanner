using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MixPlanner.Commands
{
    public abstract class AsyncCommandBase<T> : CommandBase<T>
    {
        bool isExecuting;

        protected override bool CanExecute(T parameter)
        {
            return !isExecuting && base.CanExecute(parameter);
        }

        protected abstract Task DoExecute(T parameter);

        protected async override void Execute(T parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(T parameter)
        {
            // tell the button that we're now executing...
            isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                // execute user code
                await DoExecute(parameter);
            }
            finally
            {
                // tell the button we're done
                isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }
    }

    public abstract class AsyncCommandBase : ICommand
    {
        bool isExecuting;

        public virtual bool CanExecute(object parameter)
        {
            return !isExecuting;
        }

        protected abstract Task DoExecute(object parameter);

        public async virtual void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            // tell the button that we're now executing...
            isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                // execute user code
                await DoExecute(parameter);
            }
            finally
            {
                // tell the button we're done
                isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        protected void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}