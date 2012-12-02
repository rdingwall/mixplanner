﻿using System;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class ReorderMixTrackCommand : ICommand
    {
        readonly IMix mix;

        public ReorderMixTrackCommand(IMix mix)
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
            if (parameter == null) return;

            var dropInfo = (DropInfo)parameter;

            var sourceItem = dropInfo.Data as MixItemViewModel;

            mix.Reorder(sourceItem.MixItem, dropInfo.InsertIndex);
        }

        public event EventHandler CanExecuteChanged;
    }
}