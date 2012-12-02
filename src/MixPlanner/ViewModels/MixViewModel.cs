using System;
using System.Collections.ObjectModel;
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

        public MixViewModel(IMessenger messenger, DropTrackIntoMixCommand dropTrackCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (dropTrackCommand == null) throw new ArgumentNullException("dropTrackCommand");
            this.messenger = messenger;
            DropTrackCommand = dropTrackCommand;
            Items = new ObservableCollection<MixItemViewModel>();
            messenger.Register<TrackAddedToMixEvent>(this, OnTrackAdded);
        }

        void OnTrackAdded(TrackAddedToMixEvent obj)
        {
            var trackItem = new MixItemViewModel(messenger, obj.Item);
            Items.Insert(obj.InsertIndex, trackItem);
        }

        public ICommand DropTrackCommand { get; private set; }

        public ObservableCollection<MixItemViewModel> Items { get; private set; }

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