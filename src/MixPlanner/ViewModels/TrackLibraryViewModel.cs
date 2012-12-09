﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ICommand ImportTracksCommand { get; private set; }
        public ICommand RemoveTrackFromLibraryCommand { get; private set; }
        public ObservableCollection<LibraryItemViewModel> Items { get; private set; }
        public PlayTrackCommand PlayCommand { get; private set; }

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
            ImportTracksIntoLibraryCommand importTracksCommand,
            RemoveTrackFromLibraryCommand removeTrackCommand,
            PlayTrackCommand playCommand
            )
            : base(messenger)
        {
            if (importTracksCommand == null) throw new ArgumentNullException("importTracksCommand");
            if (playCommand == null) throw new ArgumentNullException("playCommand");

            Items = new ObservableCollection<LibraryItemViewModel>();
            PlayCommand = playCommand;

            messenger.Register<TrackAddedToLibraryEvent>(this, OnTrackAddedToLibrary);
            messenger.Register<TrackRemovedFromLibraryEvent>(this, OnTrackRemoved);

            ImportTracksCommand = importTracksCommand;
            RemoveTrackFromLibraryCommand = new DelKeyEventToCommandFilter(removeTrackCommand, () => SelectedItem);
        }

        void OnTrackRemoved(TrackRemovedFromLibraryEvent e)
        {
            var item = Items.FirstOrDefault(i => e.Track.Equals(i.Track));
            Items.Remove(item);
        }

        void OnTrackAddedToLibrary(TrackAddedToLibraryEvent e)
        {
            var track = e.Track;
            var item = new LibraryItemViewModel(track);
            Items.Add(item);
        }
    }
}