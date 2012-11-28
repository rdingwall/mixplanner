using System.Collections.ObjectModel;
using System.Windows;
using GongSolutions.Wpf.DragDrop;

namespace MixPlanner.ViewModels
{
    public class MixViewModel : IDropTarget
    {
        public MixViewModel()
        {
            Items = new ObservableCollection<MixItemViewModel>();
            Items.Add(new MixItemViewModel { Text = "Opening track"});
            Items.Add(new MixItemViewModel { Text = ">>> Triple drop"});
        }

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
            var sourceItem = dropInfo.Data as LibraryItemViewModel;

            var item = new MixItemViewModel
                           {
                               Text = string.Format("{0} {1}", sourceItem.Key, sourceItem.Title),
                               Track = sourceItem.Track
                           };
            Items.Insert(dropInfo.InsertIndex, item);
        }
    }
}