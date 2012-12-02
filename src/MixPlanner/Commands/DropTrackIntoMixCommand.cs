using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Events;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class DropTrackIntoMixCommand : ICommand
    {
        readonly IMessenger messenger;

        public DropTrackIntoMixCommand(IMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dropInfo = (DropInfo) parameter;

            var sourceItem = dropInfo.Data as LibraryItemViewModel;

            messenger.Send(new TrackAddedToMixEvent(sourceItem.Track, dropInfo.InsertIndex));
        }

        public event EventHandler CanExecuteChanged;
    }
}