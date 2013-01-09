using System;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class EditTrackCommand : CommandBase<Track>
    {
        readonly SaveTrackCommand saveTrackCommand;
        readonly CloseWindowCommand closeWindowCommand;

        public EditTrackCommand(
            SaveTrackCommand saveTrackCommand,
            CloseWindowCommand closeWindowCommand)
        {
            if (saveTrackCommand == null) throw new ArgumentNullException("saveTrackCommand");
            if (closeWindowCommand == null) throw new ArgumentNullException("closeWindowCommand");
            this.saveTrackCommand = saveTrackCommand;
            this.closeWindowCommand = closeWindowCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new EditTrackViewModel(saveTrackCommand, closeWindowCommand, parameter);
            var window = new EditTrackWindow {DataContext = viewModel};
            window.ShowDialog();
        }
    }
}