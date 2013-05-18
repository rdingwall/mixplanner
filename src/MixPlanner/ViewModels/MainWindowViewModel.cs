using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MixSurroundingAreaViewModel MixSurroundingArea { get; private set; }
        public TrackLibraryViewModel TrackLibrary { get; private set; }
        public MiniPlayerViewModel MiniPlayer { get; private set; }
        public StatusBarViewModel StatusBar { get; private set; }
        public FocusSearchBoxCommand FocusSearchBoxCommand { get; private set; }

        public MainWindowViewModel(
            IMessenger messenger,
            MixSurroundingAreaViewModel mixSurroundingAreaViewModel,
            TrackLibraryViewModel trackLibraryViewModel,
            MiniPlayerViewModel miniPlayerViewModel,
            StatusBarViewModel statusBar,
            FocusSearchBoxCommand focusSearchBoxCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixSurroundingAreaViewModel == null) throw new ArgumentNullException("mixSurroundingAreaViewModel");
            if (trackLibraryViewModel == null) throw new ArgumentNullException("trackLibraryViewModel");
            if (miniPlayerViewModel == null) throw new ArgumentNullException("miniPlayerViewModel");
            if (statusBar == null) throw new ArgumentNullException("statusBar");
            if (focusSearchBoxCommand == null) throw new ArgumentNullException("focusSearchBoxCommand");

            MixSurroundingArea = mixSurroundingAreaViewModel;
            TrackLibrary = trackLibraryViewModel;
            MiniPlayer = miniPlayerViewModel;
            StatusBar = statusBar;
            FocusSearchBoxCommand = focusSearchBoxCommand;

            messenger.Register<DialogMessage>(this, OnDialogRequested);
        }

        static void OnDialogRequested(DialogMessage obj)
        {
            var result = MessageBox.Show(obj.Content, obj.Caption, obj.Button);
            obj.Callback(result);
        }
    }
}