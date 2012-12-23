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
        TrackLibraryItemViewModel selectedItem;
        public ICommand ImportFilesCommand { get; private set; }
        public ICommand RemoveDelKeyCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        readonly ObservableCollection<TrackLibraryItemViewModel> items;
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public ICommand ShowInExplorerCommand { get; private set; }
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

        public TrackLibraryItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
                RaisePropertyChanged(() => SelectedItems);
                RaisePropertyChanged(() => HasSingleItemSelected);
            }
        }

        public bool HasSingleItemSelected
        {
            get { return SelectedItems.Count() == 1; }
        }

        public TrackLibraryViewModel(
            IMessenger messenger,
            ImportFilesIntoLibraryCommand importFilesCommand,
            RemoveTracksFromLibraryCommand removeTracksCommand,
            PlayPauseTrackCommand playCommand,
            SearchCommand searchCommand,
            ShowInExplorerCommand showInExplorerCommand
            )
            : base(messenger)
        {
            if (importFilesCommand == null) throw new ArgumentNullException("importFilesCommand");
            if (removeTracksCommand == null) throw new ArgumentNullException("removeTracksCommand");
            if (playCommand == null) throw new ArgumentNullException("playCommand");
            if (searchCommand == null) throw new ArgumentNullException("searchCommand");
            if (showInExplorerCommand == null) throw new ArgumentNullException("showInExplorerCommand");

            items = new ObservableCollection<TrackLibraryItemViewModel>();
            ItemsView = CollectionViewSource.GetDefaultView(items);
            PlayPauseCommand = playCommand;

            messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
            messenger.Register<TrackRemovedFromLibraryEvent>(this, OnTrackRemoved);
            messenger.Register<SearchRequestedEvent>(this, OnSearchRequested);
            messenger.Register<SearchTextClearedEvent>(this, OnSearchTextCleared);

            SearchCommand = searchCommand;
            ShowInExplorerCommand = showInExplorerCommand;

            ImportFilesCommand = importFilesCommand;
            RemoveCommand = removeTracksCommand;
            RemoveDelKeyCommand = new DelKeyEventToCommandFilter(removeTracksCommand, () => SelectedItems);
        }

        void OnSearchTextCleared(SearchTextClearedEvent obj)
        {
            ItemsView.Filter = null;
        }

        void OnSearchRequested(SearchRequestedEvent obj)
        {
            ItemsView.Filter = new TrackLibrarySearchFilter(obj.SearchText).Filter;
        }

        public IEnumerable<TrackLibraryItemViewModel> SelectedItems
        {
            get
            { 
                return items.Where(i => i.IsSelected).ToList();
            }
        }

        void OnTrackRemoved(TrackRemovedFromLibraryEvent e)
        {
            var item = items.FirstOrDefault(i => e.Track.Equals(i.Track));
            items.Remove(item);
        }

        void OnTrackAddedToLibrary(TrackAddedToLibraryEvent e)
        {
            var track = e.Track;
            var item = new TrackLibraryItemViewModel(MessengerInstance, track);
            items.Add(item);
        }
    }
}