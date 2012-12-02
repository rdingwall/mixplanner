using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixViewModel : IDropTarget
    {
        readonly IMessenger messenger;

        public RemoveTrackFromMixCommand RemoveCommand { get; private set; }
        public ICommand DropTrackCommand { get; private set; }
        public ObservableCollection<MixItemViewModel> Items { get; private set; }
        public MixItemViewModel SelectedItem { get; set; }

        public MixViewModel(IMessenger messenger, 
            DropTrackIntoMixCommand dropTrackCommand, 
            RemoveTrackFromMixCommand removeCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (dropTrackCommand == null) throw new ArgumentNullException("dropTrackCommand");
            if (removeCommand == null) throw new ArgumentNullException("removeCommand");
            this.messenger = messenger;
            DropTrackCommand = dropTrackCommand;
            this.RemoveCommand = removeCommand;
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
            var trackItem = new MixItemViewModel(messenger, obj.Item);
            Items.Insert(obj.InsertIndex, trackItem);
        }

        public void DragOver(DropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as LibraryItemViewModel;

            if (sourceItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(DropInfo dropInfo)
        {
            DropTrackCommand.Execute(dropInfo);
        }
    }
}