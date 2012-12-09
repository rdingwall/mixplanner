using System;
using System.Windows;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class DropItemIntoMixCommand : CommandBase<DropInfo>
    {
        readonly ICommand addTrackCommand;
        readonly ICommand reorderTrackCommand;

        public DropItemIntoMixCommand(
            AddTrackToMixCommand addTrackCommand,
            ReorderMixTrackCommand reorderTrackCommand)
        {
            if (addTrackCommand == null) throw new ArgumentNullException("addTrackCommand");
            if (reorderTrackCommand == null) throw new ArgumentNullException("reorderTrackCommand");
            this.addTrackCommand = addTrackCommand;
            this.reorderTrackCommand = reorderTrackCommand;
        }

        protected override bool DoCanExecute(DropInfo parameter)
        {
            return (parameter.Data is LibraryItemViewModel ||
                    parameter.Data is MixItemViewModel);
        }

        protected override void DoExecute(DropInfo parameter)
        {
            if (parameter.Data is LibraryItemViewModel)
                addTrackCommand.Execute(parameter);

            else if (parameter.Data is MixItemViewModel)
                reorderTrackCommand.Execute(parameter);
        }
    }
}