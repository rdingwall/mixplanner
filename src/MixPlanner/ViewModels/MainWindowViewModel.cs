using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IMix mix;

        public MixSurroundingAreaViewModel MixSurroundingArea { get; private set; }
        public TrackLibraryViewModel TrackLibrary { get; private set; }
        public FocusSearchBoxCommand FocusSearchBoxCommand { get; private set; }
        public OpenMixCommand OpenCommand { get; private set; }
        public SaveMixCommand SaveCommand { get; set; }
        public AudioPlayerViewModel AudioPlayer { get; private set; }

        public IMix Mix
        {
            get { return mix; }
            private set
            {
                mix = value;
                RaisePropertyChanged(() => Mix);
            }
        }

        public MainWindowViewModel(
            IMessenger messenger,
            MixSurroundingAreaViewModel mixSurroundingAreaViewModel,
            TrackLibraryViewModel trackLibraryViewModel,
            FocusSearchBoxCommand focusSearchBoxCommand,
            OpenMixCommand openCommand,
            SaveMixCommand saveCommand,
            AudioPlayerViewModel audioPlayer) : base(messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (mixSurroundingAreaViewModel == null) throw new ArgumentNullException("mixSurroundingAreaViewModel");
            if (trackLibraryViewModel == null) throw new ArgumentNullException("trackLibraryViewModel");
            if (focusSearchBoxCommand == null) throw new ArgumentNullException("focusSearchBoxCommand");
            if (openCommand == null) throw new ArgumentNullException("openCommand");
            if (saveCommand == null) throw new ArgumentNullException("saveCommand");
            if (audioPlayer == null) throw new ArgumentNullException("audioPlayer");

            MixSurroundingArea = mixSurroundingAreaViewModel;
            TrackLibrary = trackLibraryViewModel;
            FocusSearchBoxCommand = focusSearchBoxCommand;
            OpenCommand = openCommand;
            SaveCommand = saveCommand;
            AudioPlayer = audioPlayer;

            messenger.Register<DialogMessage>(this, OnDialogRequested);
            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
        }

        void OnMixLoaded(MixLoadedEvent obj)
        {
            Mix = obj.Mix;
        }

        static void OnDialogRequested(DialogMessage obj)
        {
            var result = MessageBox.Show(obj.Content, obj.Caption, obj.Button);
            obj.Callback(result);
        }
    }
}