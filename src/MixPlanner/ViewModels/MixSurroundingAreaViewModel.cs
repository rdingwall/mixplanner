using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Commands;

namespace MixPlanner.ViewModels
{
    public class MixSurroundingAreaViewModel : ViewModelBase, IDropTarget
    {
        public MixViewModel Mix { get; private set; }
        public DragTracksOutOfMixCommand DropItemCommand { get; private set; }

        public MixSurroundingAreaViewModel(MixViewModel mix, IMessenger messenger,
                                           DragTracksOutOfMixCommand dropItemCommand)
            : base(messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (dropItemCommand == null) throw new ArgumentNullException("dropItemCommand");
            Mix = mix;
            DropItemCommand = dropItemCommand;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            Console.WriteLine("Dragging over surrounding area");

            if (!DropItemCommand.CanExecute(dropInfo))
            {
                //dropInfo.IsNotHandled = true;
                return;
            }

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (!DropItemCommand.CanExecute(dropInfo))
            {
                //dropInfo.IsNotHandled = true;
                return;
            }

            DropItemCommand.Execute(dropInfo);
        }
    }
}