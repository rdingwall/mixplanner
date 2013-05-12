using System;
using System.Threading;
using MixPlanner.Commands;

namespace MixPlanner.ProgressDialog
{
    public class CancelCommand : CommandBase
    {
        readonly CancellationTokenSource cancellationTokenSource;

        public CancelCommand(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null) throw new ArgumentNullException("cancellationTokenSource");
            this.cancellationTokenSource = cancellationTokenSource;
        }

        public override bool CanExecute(object parameter)
        {
            return !cancellationTokenSource.IsCancellationRequested;
        }

        public override void Execute(object parameter)
        {
            cancellationTokenSource.Cancel();
            RaiseCanExecuteChanged();
        }
    }
}