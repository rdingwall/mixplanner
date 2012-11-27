using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel
    {
        readonly IMessenger messenger;

        public MainWindowViewModel(IMessenger messenger, DropFilesCommand dropFilesCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (dropFilesCommand == null) throw new ArgumentNullException("dropFilesCommand");
            this.messenger = messenger;
            DropFilesCommand = dropFilesCommand;
            LibraryItems = new ObservableCollection<LibraryItemViewModel>();

            this.messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
        }

        void OnTrackAddedToLibrary(TrackAddedToLibraryEvent e)
        {
            var track = e.Track;
            var item = new LibraryItemViewModel
                           {
                               Artist = track.Artist,
                               Title = track.Title,
                               Genre = track.Genre,
                               Bpm = track.Bpm,
                               Year = track.Year,
                               Label = track.Label,
                               Filename = track.File.FullName,
                               Key = track.Key.ToString()
                           };
            LibraryItems.Add(item);
        }

        public ICommand DropFilesCommand { get; private set; }

        public ObservableCollection<LibraryItemViewModel> LibraryItems { get; private set; } 
    }
}