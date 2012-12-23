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
        public ICommand RemoveCommand { get; private set; }
        public ObservableCollection<MixItemViewModel> Items { get; private set; }
        public ICommand DropItemCommand { get; private set; }
        public ICommand DropFilesCommand { get; private set; }
        public ICommand PlayPauseCommand { get; set; }

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
            PlayOrPauseTrackCommand playOrPauseCommand) : base(messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (removeCommand == null) throw new ArgumentNullException("removeCommand");
            if (viewModels == null) throw new ArgumentNullException("viewModels");
            if (dropItemCommand == null) throw new ArgumentNullException("dropItemCommand");
            if (dropFilesCommand == null) throw new ArgumentNullException("dropFilesCommand");
            if (playOrPauseCommand == null) throw new ArgumentNullException("playOrPauseCommand");
            DropItemCommand = dropItemCommand;
            DropFilesCommand = dropFilesCommand;
            PlayPauseCommand = playOrPauseCommand;
            this.viewModels = viewModels;
            RemoveCommand = removeCommand;
            RemoveDelKeyCommand = new DelKeyEventToCommandFilter(removeCommand, () => SelectedItems);
            Items = new ObservableCollection<MixItemViewModel>();
            messenger.Register<TrackAddedToMixEvent>(this, OnTrackAdded);
            messenger.Register<TrackRemovedFromMixEvent>(this, OnTrackRemoved);
        }

        public IEnumerable<MixItem> SelectedItems
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

        public void DragOver(DropInfo dropInfo)
        {
            if (!DropItemCommand.CanExecute(dropInfo))
                return;

            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(DropInfo dropInfo)
        {
            DropItemCommand.Execute(dropInfo);
        }
    }
}