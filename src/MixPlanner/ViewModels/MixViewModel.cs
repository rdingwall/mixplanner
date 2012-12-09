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
using MixPlanner.Player;

namespace MixPlanner.ViewModels
{
    public class MixViewModel : ViewModelBase, IDropTarget
    {
        MixItemViewModel selectedItem;
        readonly IAudioPlayer player;
        public RemoveTrackFromMixCommand RemoveCommand { get; private set; }
        public ReorderMixTrackCommand ReorderTrackCommand { get; set; }
        public ICommand DropTrackCommand { get; private set; }
        public ObservableCollection<MixItemViewModel> Items { get; private set; }

        public MixItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value;
            RaisePropertyChanged(() => SelectedItem);}
        }

        public MixViewModel(IMessenger messenger, 
            DropTrackIntoMixCommand dropTrackCommand, 
            RemoveTrackFromMixCommand removeCommand,
            IAudioPlayer player,
            ReorderMixTrackCommand reorderTrackCommand
            ) : base(messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (dropTrackCommand == null) throw new ArgumentNullException("dropTrackCommand");
            if (removeCommand == null) throw new ArgumentNullException("removeCommand");
            if (player == null) throw new ArgumentNullException("player");
            if (reorderTrackCommand == null) throw new ArgumentNullException("reorderTrackCommand");
            DropTrackCommand = dropTrackCommand;
            RemoveCommand = removeCommand;
            ReorderTrackCommand = reorderTrackCommand;
            this.player = player;
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
            var trackItem = new MixItemViewModel(MessengerInstance, obj.Item, 
                new PlayOrPauseTrackCommand(player, MessengerInstance, obj.Item.Track));
            Items.Insert(obj.InsertIndex, trackItem);
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
                DropTrackCommand.Execute(dropInfo);
            else if (dropInfo.Data is MixItemViewModel)
                ReorderTrackCommand.Execute(dropInfo);
        }
    }
}