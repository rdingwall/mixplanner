using System;
using System.Threading;
using MixPlanner.ViewModels;
using MixPlanner.Views;

namespace MixPlanner.ProgressDialog
{
    public interface IProgressDialogService
    {
        void Execute(TaskFactory taskFactory, string windowTitle, string label);
        bool TryExecute<T>(TaskFactory<T> taskFactory, string windowTitle, 
            string label, out T result);
    }

    public class ProgressDialogService : IProgressDialogService
    {
        public void Execute(TaskFactory taskFactory, string windowTitle, string label)
        {
            if (taskFactory == null) throw new ArgumentNullException("taskFactory");
            if (windowTitle == null) throw new ArgumentNullException("windowTitle");
            if (label == null) throw new ArgumentNullException("label");

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                var cancelCommand = new CancelCommand(cancellationTokenSource);
                var viewModel = new ProgressDialogWindowViewModel(
                    windowTitle, label, cancellationToken, cancelCommand);

                var window = new ProgressDialogWindow
                                 {
                                     DataContext = viewModel
                                 };

                taskFactory(viewModel.Progress, cancellationToken)
                    .ContinueWith(_ => viewModel.Close = true);

                window.ShowDialog();
            }
        }

        public bool TryExecute<T>(TaskFactory<T> taskFactory, string windowTitle, string label,
            out T result)
        {
            if (taskFactory == null) throw new ArgumentNullException("taskFactory");
            if (windowTitle == null) throw new ArgumentNullException("windowTitle");
            if (label == null) throw new ArgumentNullException("label");

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                var cancelCommand = new CancelCommand(cancellationTokenSource);
                var viewModel = new ProgressDialogWindowViewModel(
                    windowTitle, label, cancellationToken, cancelCommand);

                var window = new ProgressDialogWindow
                {
                    DataContext = viewModel
                };

                var task = taskFactory(viewModel.Progress, cancellationToken);

                task.ContinueWith(_ => viewModel.Close = true);

                window.ShowDialog();

                if (task.IsCompleted)
                {
                    result = task.Result;
                    return true;
                }

                result = default(T);
                return false;
            }
        }
    }
}