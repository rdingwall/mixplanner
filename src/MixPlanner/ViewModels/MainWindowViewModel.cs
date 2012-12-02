using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        LibraryItemViewModel selectedItem;
        public ICommand DropFilesCommand { get; private set; }
        public ICommand RemoveTrackFromLibraryCommand { get; private set; }

        public ObservableCollection<LibraryItemViewModel> LibraryItems { get; private set; }

        public LibraryItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public MixViewModel Mix { get; private set; }
        public PlayTrackCommand PlayCommand { get; set; }

        public MainWindowViewModel(
            IMessenger messenger,
            DropFilesCommand dropFilesCommand,
            RemoveTrackFromLibraryCommand removeTrackCommand,
            MixViewModel mixViewModel,
            PlayTrackCommand playCommand
            )
            : base(messenger)
        {
            if (dropFilesCommand == null) throw new ArgumentNullException("dropFilesCommand");
            if (mixViewModel == null) throw new ArgumentNullException("mixViewModel");
            if (playCommand == null) throw new ArgumentNullException("playCommand");

            LibraryItems = new ObservableCollection<LibraryItemViewModel>();
            Mix = mixViewModel;
            PlayCommand = playCommand;

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