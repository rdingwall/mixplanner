using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.AttachedProperties;
using MixPlanner.Commands;
using MixPlanner.Converters;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MoreLinq;

namespace MixPlanner.ViewModels
{
    public class TrackLibraryViewModel : ViewModelBase
    {
        TrackLibraryItemViewModel selectedItem;
        readonly ObservableCollection<TrackLibraryItemViewModel> items;

        public ImportFilesIntoLibraryCommand ImportFilesCommand { get; private set; }
        public KeyEventProxyCommand RemoveDelKeyCommand { get; private set; }
        public KeyEventProxyCommand PlayPauseSpaceKeyCommand { get; private set; }
        public RemoveTracksFromLibraryCommand RemoveCommand { get; private set; }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public ShowInExplorerCommand ShowInExplorerCommand { get; private set; }
        public SearchLibraryCommand SearchCommand { get; private set; }
        public OpenSettingsCommand OpenSettingsCommand { get; private set; }
        public EditTrackCommand EditTrackCommand { get; private set; }
        public KeyEventProxyCommand EditF2KeyCommand { get; private set; }

        public ObservableCollection<GridViewColumn> LibraryColumns { get; private set; }
            
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
            SearchLibraryCommand searchCommand,
            ShowInExplorerCommand showInExplorerCommand,
            OpenSettingsCommand openSettingsCommand,
            EditTrackCommand editTrackCommand
            )
            : base(messenger)
        {
            if (importFilesCommand == null) throw new ArgumentNullException("importFilesCommand");
            if (removeTracksCommand == null) throw new ArgumentNullException("removeTracksCommand");
            if (playCommand == null) throw new ArgumentNullException("playCommand");
            if (searchCommand == null) throw new ArgumentNullException("searchCommand");
            if (showInExplorerCommand == null) throw new ArgumentNullException("showInExplorerCommand");
            if (openSettingsCommand == null) throw new ArgumentNullException("openSettingsCommand");
            if (editTrackCommand == null) throw new ArgumentNullException("editTrackCommand");

            items = new ObservableCollection<TrackLibraryItemViewModel>();
            ItemsView = CollectionViewSource.GetDefaultView(items);
            PlayPauseCommand = playCommand;

            LibraryColumns = new ObservableCollection<GridViewColumn>
                          {
                              new GridViewColumn { Header = "Transition", DisplayMemberBinding = new Binding("Transition") { Converter = new TransitionDescriptionConverter() }, Width= 150},
                              new GridViewColumn { Header = "IncreaseRequired", DisplayMemberBinding = new Binding("IncreaseRequired") { Converter = new PlaySpeedIncreaseConverter() }, Width= 100},
                              new GridViewColumn { Header = "Bpm", DisplayMemberBinding = new Binding("Bpm"), Width= 100},
                              new GridViewColumn { Header = "Key", DisplayMemberBinding = new Binding("Key") { Converter = new HarmonicKeyCoverter() }, Width= 100},
                              new GridViewColumn { Header = "Artist", DisplayMemberBinding = new Binding("Artist"), Width= 100},
                              new GridViewColumn { Header = "Title", DisplayMemberBinding = new Binding("Title"), Width= 100},
                              new GridViewColumn { Header = "Year", DisplayMemberBinding = new Binding("Year"), Width= 100},
                              new GridViewColumn { Header = "Genre", DisplayMemberBinding = new Binding("Genre"), Width= 100},
                              new GridViewColumn { Header = "Label", DisplayMemberBinding = new Binding("Label"), Width= 100},
                              new GridViewColumn { Header = "Filename", DisplayMemberBinding = new Binding("Filename")}
                          };

            LibraryColumns.ForEach(c => GridViewSort.SetPropertyName(c, (string)c.Header));

            messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
            messenger.Register<TrackRemovedFromLibraryEvent>(this, OnTrackRemoved);
            messenger.Register<SearchRequestedEvent>(this, OnSearchRequested);
            messenger.Register<SearchTextClearedEvent>(this, OnSearchTextCleared);

            SearchCommand = searchCommand;
            ShowInExplorerCommand = showInExplorerCommand;
            OpenSettingsCommand = openSettingsCommand;
            EditTrackCommand = editTrackCommand;

            ImportFilesCommand = importFilesCommand;
            RemoveCommand = removeTracksCommand;
            RemoveDelKeyCommand = new KeyEventProxyCommand(
                removeTracksCommand, () => SelectedTracks, Key.Delete, Key.Back);
            PlayPauseSpaceKeyCommand = new KeyEventProxyCommand(
                PlayPauseCommand, () => SelectedItem.Track, Key.Space, Key.Return, Key.Enter);
            EditF2KeyCommand = new KeyEventProxyCommand(EditTrackCommand, () => SelectedItem.Track, Key.F2);
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
            var track = e.Track;
            var item = new TrackLibraryItemViewModel(MessengerInstance, track);
            items.Add(item);
        }
    }
}