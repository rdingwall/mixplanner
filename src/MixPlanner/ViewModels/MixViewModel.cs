using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixViewModel : ViewModelBase, IDropTarget
    {
        MixItemViewModel selectedItem;
        readonly IMixItemViewModelFactory viewModels;

        public KeyEventProxyCommand RemoveDelKeyCommand { get; private set; }
        public KeyEventProxyCommand PlayPauseSpaceKeyCommand { get; private set; }
        public RemoveTracksFromMixCommand RemoveCommand { get; private set; }
        public ObservableCollection<MixItemViewModel> Items { get; private set; }
        public DropItemIntoMixCommand DropItemCommand { get; private set; }
        public ImportFilesIntoMixCommand DropFilesCommand { get; private set; }
        public PlayPauseTrackCommand PlayPauseCommand { get; private set; }
        public ResetPlaybackSpeedCommand ResetPlaybackSpeedCommand { get; private set; }
        public GetRecommendationsCommand GetRecommendationsCommand { get; private set; }
        public ClearRecommendationsCommand ClearRecommendationsCommand { get; private set; }
        public KeyEventProxyCommand EditF2KeyCommand { get; private set; }
        public EditTrackCommand EditTrackCommand { get; private set; }

        public MixItemViewModel SelectedItem
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

        public MixViewModel(IMessenger messenger, 
            RemoveTracksFromMixCommand removeCommand,
            IMixItemViewModelFactory viewModels,
            DropItemIntoMixCommand dropItemCommand,
            ImportFilesIntoMixCommand dropFilesCommand,
            PlayPauseTrackCommand playPauseCommand,
            ResetPlaybackSpeedCommand resetPlaybackSpeedCommand,
            ClearRecommendationsCommand clearRecommendationsCommand,
            GetRecommendationsCommand getRecommendationsCommand,
            EditTrackCommand editTrackCommand) : base(messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (removeCommand == null) throw new ArgumentNullException("removeCommand");
            if (viewModels == null) throw new ArgumentNullException("viewModels");
            if (dropItemCommand == null) throw new ArgumentNullException("dropItemCommand");
            if (dropFilesCommand == null) throw new ArgumentNullException("dropFilesCommand");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            if (resetPlaybackSpeedCommand == null) throw new ArgumentNullException("resetPlaybackSpeedCommand");
            if (clearRecommendationsCommand == null) throw new ArgumentNullException("clearRecommendationsCommand");
            if (getRecommendationsCommand == null) throw new ArgumentNullException("getRecommendationsCommand");
            if (editTrackCommand == null) throw new ArgumentNullException("editTrackCommand");
            DropItemCommand = dropItemCommand;
            DropFilesCommand = dropFilesCommand;
            PlayPauseCommand = playPauseCommand;
            ResetPlaybackSpeedCommand = resetPlaybackSpeedCommand;
            ClearRecommendationsCommand = clearRecommendationsCommand;
            GetRecommendationsCommand = getRecommendationsCommand;
            EditTrackCommand = editTrackCommand;
            this.viewModels = viewModels;
            RemoveCommand = removeCommand;
            RemoveDelKeyCommand = new KeyEventProxyCommand(
                removeCommand, () => SelectedItems, Key.Delete, Key.Back);
            PlayPauseSpaceKeyCommand = new KeyEventProxyCommand(
                PlayPauseCommand, () => SelectedItem.Track, Key.Space, Key.Enter, Key.Return);
            EditF2KeyCommand = new KeyEventProxyCommand(
                EditTrackCommand, () => SelectedItem.Track, Key.F2);
            Items = new ObservableCollection<MixItemViewModel>();
            messenger.Register<TrackAddedToMixEvent>(this, OnTrackAdded);
            messenger.Register<TrackRemovedFromMixEvent>(this, OnTrackRemoved);
            messenger.Register<ConfigSavedEvent>(this, _ => OnSelectionChanged());
            messenger.Register<PlaybackSpeedAdjustedEvent>(this, _ => OnSelectionChanged());
        }


        public ICollection<MixItem> SelectedItems
        {
            get
            {
                return Items
                    .Where(i => i.IsSelected)
                    .Select(i => i.MixItem)
                    .ToList();
            }
        }

        void OnTrackRemoved(TrackRemovedFromMixEvent obj)
        {
            var viewModel = Items.First(v => v.MixItem.Equals(obj.Item));
            Items.Remove(viewModel);
        }

        void OnTrackAdded(TrackAddedToMixEvent obj)
        {
            var viewModel = viewModels.CreateFor(obj.Item);
            Items.Insert(obj.InsertIndex, viewModel);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (!DropItemCommand.CanExecute(dropInfo))
                return;

            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            DropItemCommand.Execute(dropInfo);
        }

        public void OnSelectionChanged()
        {
            ClearRecommendationsCommand.Execute(null);
            GetRecommendationsCommand.Execute(SelectedItems);
        }
    }
}