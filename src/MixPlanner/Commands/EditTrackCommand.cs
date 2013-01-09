using System;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class EditTrackCommand : CommandBase<Track>
    {
        readonly CloseWindowCommand closeWindowCommand;

        public EditTrackCommand(CloseWindowCommand closeWindowCommand)
        {
            if (closeWindowCommand == null) throw new ArgumentNullException("closeWindowCommand");
            this.closeWindowCommand = closeWindowCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new EditTrackViewModel(closeWindowCommand, parameter);
            var window = new EditTrackWindow {DataContext = viewModel};
            window.ShowDialog();
        }
    }
}