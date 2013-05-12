using System;
using System.Threading;
using GalaSoft.MvvmLight.Threading;
using MixPlanner.ProgressDialog;

namespace MixPlanner.ViewModels
{
    public class ProgressDialogWindowViewModel : CloseableViewModelBase
    {
        string windowTitle;
        string label;
        string subLabel;

        public string WindowTitle
        {
            get { return windowTitle; }
            private set
            {
                windowTitle = value;
                RaisePropertyChanged(() => WindowTitle);
            }
        }

        public string Label
        {
            get { return label; }
            private set
            {
                label = value;
                RaisePropertyChanged(() => Label);
            }
        }

        public string SubLabel
        {
            get { return subLabel; }
            private set
            {
                subLabel = value;
                RaisePropertyChanged(() => SubLabel);
            }
        }

        public CancelCommand CancelCommand { get; private set; }
        public IProgress<string> Progress { get; private set; }

        public ProgressDialogWindowViewModel(
            string windowTitle,
            string label,
            CancellationToken cancellationToken,
            CancelCommand cancelCommand)
        {
            if (windowTitle == null) throw new ArgumentNullException("windowTitle");
            if (label == null) throw new ArgumentNullException("label");
            if (cancelCommand == null) throw new ArgumentNullException("cancelCommand");
            WindowTitle = windowTitle;
            Label = label;
            CancelCommand = cancelCommand;
            cancellationToken.Register(OnCancelled);
            Progress = new Progress<string>(OnProgress);
        }

        void OnCancelled()
        {
            if (DispatcherHelper.UIDispatcher != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() => Close = true);
            else
                Close = true;
        }

        void OnProgress(string obj)
        {
            if (DispatcherHelper.UIDispatcher != null)
                DispatcherHelper.CheckBeginInvokeOnUI(() => SubLabel = obj);
            else
                SubLabel = obj;
        }
    }
}