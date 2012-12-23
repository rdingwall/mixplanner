using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryViewModel : ViewModelBase
    {
        LibraryItemViewModel selectedItem;
        public ICommand ImportFilesCommand { get; private set; }
        public ICommand RemoveTracksFromLibraryCommand { get; private set; }
        readonly ObservableCollection<LibraryItemViewModel> items;
        public PlayTrackCommand PlayCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }

        ICollectionView itemsView;
        public ICollectionView ItemsView
        {
            get { return itemsView; }
            set
            {
                itemsView = value;
                RaisePropertyChanged(() => ItemsView);
            }
        }

        public LibraryItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public TrackLibraryViewModel(
            IMessenger messenger,
            ImportFilesIntoLibraryCommand importFilesCommand,
            RemoveTracksFromLibraryCommand removeTracksCommand,
            PlayTrackCommand playCommand,
            SearchCommand searchCommand
            )
            : base(messenger)
        {
            if (importFilesCommand == null) throw new ArgumentNullException("importFilesCommand");
            if (removeTracksCommand == null) throw new ArgumentNullException("removeTracksCommand");
            if (playCommand == null) throw new ArgumentNullException("playCommand");
            if (searchCommand == null) throw new ArgumentNullException("searchCommand");

            items = new ObservableCollection<LibraryItemViewModel>();
            ItemsView = CollectionViewSource.GetDefaultView(items);
            PlayCommand = playCommand;

            messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
            messenger.Register<TrackRemovedFromLibraryEvent>(this, OnTrackRemoved);
            messenger.Register<SearchRequestedEvent>(this, OnSearchRequested);
            messenger.Register<SearchTextClearedEvent>(this, OnSearchTextCleared);

            SearchCommand = searchCommand;

            ImportFilesCommand = importFilesCommand;
            RemoveTracksFromLibraryCommand = new DelKeyEventToCommandFilter(removeTracksCommand, GetSelectedItems);
        }

        void OnSearchTextCleared(SearchTextClearedEvent obj)
        {
            ItemsView.Filter = null;
        }

        void OnSearchRequested(SearchRequestedEvent obj)
        {
            ItemsView.Filter = new TrackSearchFilter(obj.SearchText).Filter;
        }

        IEnumerable<LibraryItemViewModel> GetSelectedItems()
        {
            return items.Where(i => i.IsSelected).ToList();
        }

        void OnTrackRemoved(TrackRemovedFromLibraryEvent e)
        {
            var item = items.FirstOrDefault(i => e.Track.Equals(i.Track));
            items.Remove(item);
        }

        void OnTrackAddedToLibrary(TrackAddedToLibraryEvent e)
        {
            var track = e.Track;
            var item = new LibraryItemViewModel(track);
            items.Add(item);
        }
    }
}