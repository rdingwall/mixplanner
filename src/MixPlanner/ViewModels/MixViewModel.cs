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
        public ICommand RemoveDelKeyCommand { get; private set; }
        public ICommand PlayPauseSpaceKeyCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ObservableCollection<MixItemViewModel> Items { get; private set; }
        public ICommand DropItemCommand { get; private set; }
        public ICommand DropFilesCommand { get; private set; }
        public ICommand PlayPauseCommand { get; private set; }
        public ICommand ResetPlaybackSpeedCommand { get; private set; }
        public ICommand GetRecommendationsCommand { get; private set; }
        public ICommand ClearRecommendationsCommand { get; private set; }

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
            GetRecommendationsCommand getRecommendationsCommand) : base(messenger)
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
            DropItemCommand = dropItemCommand;
            DropFilesCommand = dropFilesCommand;
            PlayPauseCommand = playPauseCommand;
            ResetPlaybackSpeedCommand = resetPlaybackSpeedCommand;
            ClearRecommendationsCommand = clearRecommendationsCommand;
            GetRecommendationsCommand = getRecommendationsCommand;
            this.viewModels = viewModels;
            RemoveCommand = removeCommand;
            RemoveDelKeyCommand = new KeyEventCommandFilter(
                removeCommand, () => SelectedItems, Key.Delete, Key.Back);
            PlayPauseSpaceKeyCommand = new KeyEventCommandFilter(
                PlayPauseCommand, () => SelectedItem.Track, Key.Space, Key.Enter, Key.Return);
            Items = new ObservableCollection<MixItemViewModel>();
            messenger.Register<TrackAddedToMixEvent>(this, OnTrackAdded);
            messenger.Register<TrackRemovedFromMixEvent>(this, OnTrackRemoved);
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