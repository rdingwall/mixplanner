using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MixViewModel Mix { get; private set; }
        public TrackLibraryViewModel TrackLibrary { get; private set; }
        public MiniPlayerViewModel MiniPlayer { get; private set; }
        public StatusBarViewModel StatusBar { get; private set; }

        public MainWindowViewModel(
            IMessenger messenger,
            MixViewModel mixViewModel,
            TrackLibraryViewModel trackLibraryViewModel,
            MiniPlayerViewModel miniPlayerViewModel,
            StatusBarViewModel statusBar)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixViewModel == null) throw new ArgumentNullException("mixViewModel");
            if (trackLibraryViewModel == null) throw new ArgumentNullException("trackLibraryViewModel");
            if (miniPlayerViewModel == null) throw new ArgumentNullException("miniPlayerViewModel");
            if (statusBar == null) throw new ArgumentNullException("statusBar");

            Mix = mixViewModel;
            TrackLibrary = trackLibraryViewModel;
            MiniPlayer = miniPlayerViewModel;
            StatusBar = statusBar;

            messenger.Register<DialogMessage>(this, OnDialogRequested);
        }

        static void OnDialogRequested(DialogMessage obj)
        {
            var result = MessageBox.Show(obj.Content, obj.Caption, obj.Button);
            obj.Callback(result);
        }
    }
}