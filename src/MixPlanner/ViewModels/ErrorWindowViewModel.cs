using System;
using GalaSoft.MvvmLight;

namespace MixPlanner.ViewModels
{
    public class ErrorWindowViewModel : ViewModelBase
    {
        public ErrorWindowViewModel(Exception exception)
        {
            if (exception == null) return;

            Message = exception.Message;
            StackTrace = exception.ToString();
        }

        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}