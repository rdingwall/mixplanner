using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public StatusBarViewModel(IMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            messenger.Register<BeganScanningDirectoryEvent>(this, _ => Message = "Scanning directory...");
            messenger.Register<BeganLoadingTracksEvent>(this, _ => Message = "Importing tracks...");
            messenger.Register<FinishedLoadingTracksEvent>(this, _ => Message = "");
        }
    }
}