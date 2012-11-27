using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand DropFilesCommand { get; private set; }
        public ICommand RemoveTrackFromLibraryCommand { get; private set; }

        public ObservableCollection<LibraryItemViewModel> LibraryItems { get; private set; }
        public LibraryItemViewModel SelectedTrack { get; set; }

        public MainWindowViewModel(
            IMessenger messenger, 
            DropFilesCommand dropFilesCommand,
            RemoveTrackFromLibraryCommand removeTrackCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (dropFilesCommand == null) throw new ArgumentNullException("dropFilesCommand");

            LibraryItems = new ObservableCollection<LibraryItemViewModel>();
            
            messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
            messenger.Register<TrackRemovedFromLibraryEvent>(this, OnTrackRemoved);

            DropFilesCommand = dropFilesCommand;
            RemoveTrackFromLibraryCommand = removeTrackCommand;
        }

        void OnTrackRemoved(TrackRemovedFromLibraryEvent e)
        {
            var item = LibraryItems.FirstOrDefault(i => e.Track.Equals(i.Track));
            LibraryItems.Remove(item);
        }

        void OnTrackAddedToLibrary(TrackAddedToLibraryEvent e)
        {
            var track = e.Track;
            var item = new LibraryItemViewModel(track);
            LibraryItems.Add(item);
        }
    }
}