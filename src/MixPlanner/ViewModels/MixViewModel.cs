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
        public MixViewModel(IMessenger messenger, DropTrackIntoMixCommand dropTrackCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (dropTrackCommand == null) throw new ArgumentNullException("dropTrackCommand");
            DropTrackCommand = dropTrackCommand;
            Items = new ObservableCollection<MixItemViewModel>();
            messenger.Register<TrackAddedToMixEvent>(this, OnTrackAdded);
        }

        void OnTrackAdded(TrackAddedToMixEvent obj)
        {
            var track = obj.Track;

            var item = new MixItemViewModel
            {
                Text = string.Format("{0} {1}", track.Key, track.Title),
                Track = track
            };
            Items.Insert(obj.InsertIndex, item);
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