using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixViewModel : ViewModelBase, IDropTarget
    {
        MixItemViewModel selectedItem;
        readonly IMixItemViewModelFactory viewModels;
        public ICommand RemoveTrackCommand { get; private set; }
        public ReorderMixTrackCommand ReorderTrackCommand { get; set; }
        public ICommand AddTrackCommand { get; private set; }
        public ObservableCollection<MixItemViewModel> Items { get; private set; }

        public MixItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public MixViewModel(IMessenger messenger, 
            AddTrackToMixCommand addTrackToMixCommand, 
            RemoveTrackFromMixCommand removeCommand,
            ReorderMixTrackCommand reorderTrackCommand,
            IMixItemViewModelFactory viewModels) : base(messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (addTrackToMixCommand == null) throw new ArgumentNullException("addTrackToMixCommand");
            if (removeCommand == null) throw new ArgumentNullException("removeCommand");
            if (reorderTrackCommand == null) throw new ArgumentNullException("reorderTrackCommand");
            if (viewModels == null) throw new ArgumentNullException("viewModels");
            this.viewModels = viewModels;
            AddTrackCommand = addTrackToMixCommand;
            RemoveTrackCommand = new DelKeyEventToCommandFilter(removeCommand, () => SelectedItem.MixItem);
            ReorderTrackCommand = reorderTrackCommand;
            Items = new ObservableCollection<MixItemViewModel>();
            messenger.Register<TrackAddedToMixEvent>(this, OnTrackAdded);
            messenger.Register<TrackRemovedFromMixEvent>(this, OnTrackRemoved);
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
            if (dropInfo.Data is LibraryItemViewModel || dropInfo.Data is MixItemViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(DropInfo dropInfo)
        {
            if (dropInfo.Data is LibraryItemViewModel)
                AddTrackCommand.Execute(dropInfo);
            else if (dropInfo.Data is MixItemViewModel)
                ReorderTrackCommand.Execute(dropInfo);
        }
    }
}