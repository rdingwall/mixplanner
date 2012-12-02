using System;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class DropTrackIntoMixCommand : ICommand
    {
        readonly IMix mix;

        public DropTrackIntoMixCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dropInfo = (DropInfo) parameter;

            var sourceItem = dropInfo.Data as LibraryItemViewModel;


            mix.Insert(sourceItem.Track, dropInfo.InsertIndex);
        }

        public event EventHandler CanExecuteChanged;
    }
}