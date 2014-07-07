namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public abstract class AsyncCommandBase<T> : CommandBase<T>
    {
        private bool isExecuting;

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

        protected override bool CanExecute(T parameter)
        {
            return !this.isExecuting && base.CanExecute(parameter);
        }

        protected abstract Task DoExecute(T parameter);

        protected async override void Execute(T parameter)
        {
            await this.ExecuteAsync(parameter);
        }
    }

    public abstract class AsyncCommandBase : ICommand
    {
        private bool isExecuting;

        public event EventHandler CanExecuteChanged = delegate { };

        public virtual bool CanExecute(object parameter)
        {
            return !isExecuting;
        }

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

        protected abstract Task DoExecute(object parameter);

        protected void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}