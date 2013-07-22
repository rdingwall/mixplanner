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
        public FocusSearchBoxCommand FocusSearchBoxCommand { get; private set; }
        public AudioPlayerViewModel AudioPlayer { get; private set; }

        public MainWindowViewModel(
            IMessenger messenger,
            MixSurroundingAreaViewModel mixSurroundingAreaViewModel,
            TrackLibraryViewModel trackLibraryViewModel,
            FocusSearchBoxCommand focusSearchBoxCommand,
            AudioPlayerViewModel audioPlayer)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixSurroundingAreaViewModel == null) throw new ArgumentNullException("mixSurroundingAreaViewModel");
            if (trackLibraryViewModel == null) throw new ArgumentNullException("trackLibraryViewModel");
            if (focusSearchBoxCommand == null) throw new ArgumentNullException("focusSearchBoxCommand");
            if (audioPlayer == null) throw new ArgumentNullException("audioPlayer");

            MixSurroundingArea = mixSurroundingAreaViewModel;
            TrackLibrary = trackLibraryViewModel;
            FocusSearchBoxCommand = focusSearchBoxCommand;
            AudioPlayer = audioPlayer;

            messenger.Register<DialogMessage>(this, OnDialogRequested);
        }

        static void OnDialogRequested(DialogMessage obj)
        {
            var result = MessageBox.Show(obj.Content, obj.Caption, obj.Button);
            obj.Callback(result);
        }
    }
}