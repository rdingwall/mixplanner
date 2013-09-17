using System;
using System.Windows;
using Castle.Windsor;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.Commands;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class MixSurroundingAreaViewModel : ViewModelBase, IDropTarget
    {
        readonly IWindsorContainer container;
        MixViewModel mix;

        public MixViewModel Mix
        {
            get { return mix; }
            private set
            {
                mix = value;
                RaisePropertyChanged(() => Mix);
            }
        }

        public DragTracksOutOfMixCommand DropItemCommand { get; private set; }

        public MixSurroundingAreaViewModel(
            IWindsorContainer container, 
            IMessenger messenger,
            DragTracksOutOfMixCommand dropItemCommand)
            : base(messenger)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (dropItemCommand == null) throw new ArgumentNullException("dropItemCommand");
            this.container = container;
            DropItemCommand = dropItemCommand;

            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
        }

        void OnMixLoaded(MixLoadedEvent obj)
        {
            Mix = container.Resolve<MixViewModel>(new {mix = obj.Mix});
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