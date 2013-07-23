using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.AttachedProperties;
using MixPlanner.Commands;
using MixPlanner.Converters;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MoreLinq;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryViewModel : ViewModelBase, IDragSource
    {
        readonly QuickEditBpmCommand quickEditBpmCommand;
        readonly QuickEditHarmonicKeyCommand quickEditHarmonicKeyCommand;
        TrackLibraryItemViewModel selectedItem;
        readonly ObservableCollection<TrackLibraryItemViewModel> items;

        public ImportFilesIntoLibraryCommand ImportFilesCommand { get; private set; }
        public RemoveTracksFromLibraryCommand RemoveCommand { get; private set; }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public ShowInExplorerCommand ShowInExplorerCommand { get; private set; }
        public SearchLibraryCommand SearchCommand { get; private set; }
        public OpenSettingsCommand OpenSettingsCommand { get; private set; }
        public EditTrackCommand EditTrackCommand { get; private set; }

        bool isSearchBoxFocused;
        public bool IsSearchBoxFocused
        {
            get { return isSearchBoxFocused; }
            set
            {
                isSearchBoxFocused = value;
                RaisePropertyChanged(() => IsSearchBoxFocused);
            }
        }

        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChanged(() => SearchText);
            }
        }

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
                RaisePropertyChanged(() => SelectedTrack);
            }
        }

        public Track SelectedTrack
        {
            get { return SelectedItem != null ? SelectedItem.Track : null; }
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
            SearchLibraryCommand searchCommand,
            ShowInExplorerCommand showInExplorerCommand,
            OpenSettingsCommand openSettingsCommand,
            EditTrackCommand editTrackCommand,
            QuickEditBpmCommand quickEditBpmCommand,
            QuickEditHarmonicKeyCommand quickEditHarmonicKeyCommand)
            : base(messenger)
        {
            
            if (importFilesCommand == null) throw new ArgumentNullException("importFilesCommand");
            if (removeTracksCommand == null) throw new ArgumentNullException("removeTracksCommand");
            if (playCommand == null) throw new ArgumentNullException("playCommand");
            if (searchCommand == null) throw new ArgumentNullException("searchCommand");
            if (showInExplorerCommand == null) throw new ArgumentNullException("showInExplorerCommand");
            if (openSettingsCommand == null) throw new ArgumentNullException("openSettingsCommand");
            if (editTrackCommand == null) throw new ArgumentNullException("editTrackCommand");
            if (quickEditBpmCommand == null) throw new ArgumentNullException("quickEditBpmCommand");

            items = new ObservableCollection<TrackLibraryItemViewModel>();
            ItemsView = CollectionViewSource.GetDefaultView(items);
            PlayPauseCommand = playCommand;
            this.quickEditBpmCommand = quickEditBpmCommand;
            this.quickEditHarmonicKeyCommand = quickEditHarmonicKeyCommand;

            messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
            messenger.Register<TrackRemovedFromLibraryEvent>(this, OnTrackRemoved);
            messenger.Register<SearchRequestedEvent>(this, OnSearchRequested);
            messenger.Register<SearchTextClearedEvent>(this, OnSearchTextCleared);
            messenger.Register<SearchBoxFocusRequestedEvent>(this, OnSearchBoxFocusRequested);
            messenger.Register<TrackLibraryLoadedEvent>(this, OnTrackLibraryLoaded);

            SearchCommand = searchCommand;
            ShowInExplorerCommand = showInExplorerCommand;
            OpenSettingsCommand = openSettingsCommand;
            EditTrackCommand = editTrackCommand;

            ImportFilesCommand = importFilesCommand;
            RemoveCommand = removeTracksCommand;
        }

        void OnTrackLibraryLoaded(TrackLibraryLoadedEvent obj)
        {
            items.Clear();
            obj.Tracks.ForEach(t => items.Add(CreateItemViewModel(t)));
        }

        void OnSearchBoxFocusRequested(SearchBoxFocusRequestedEvent obj)
        {
            SearchText = "";
            IsSearchBoxFocused = true;
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

        public IEnumerable<Track> SelectedTracks
        {
            get
            {
                return items.Where(i => i.IsSelected).Select(i => i.Track).ToList();
            }
        }

        void OnTrackRemoved(TrackRemovedFromLibraryEvent e)
        {
            var item = items.FirstOrDefault(i => e.Track.Equals(i.Track));
            items.Remove(item);
        }

        void OnTrackAddedToLibrary(TrackAddedToLibraryEvent e)
        {
            Track track = e.Track;
            TrackLibraryItemViewModel item = CreateItemViewModel(track);
            items.Add(item);
        }

        public void OnSelectionChanged()
        {
            RaisePropertyChanged(() => SelectedItems);
            RaisePropertyChanged(() => SelectedTracks);
        }

        TrackLibraryItemViewModel CreateItemViewModel(Track track)
        {
            return new TrackLibraryItemViewModel(MessengerInstance, track, quickEditBpmCommand, quickEditHarmonicKeyCommand);
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            dragInfo.Data = SelectedItems;
            dragInfo.Effects = DragDropEffects.Move | DragDropEffects.Copy;
        }

        public void Dropped(IDropInfo dropInfo)
        {
            
        }
    }
}