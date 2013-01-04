using System.Threading.Tasks;

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
}