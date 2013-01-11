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
        readonly ReloadTrackFileCommand reloadTrackFileCommand;

        public EditTrackCommand(
            SaveTrackCommand saveTrackCommand,
            CloseWindowCommand closeWindowCommand,
            ReloadTrackFileCommand reloadTrackFileCommand)
        {
            if (saveTrackCommand == null) throw new ArgumentNullException("saveTrackCommand");
            if (closeWindowCommand == null) throw new ArgumentNullException("closeWindowCommand");
            if (reloadTrackFileCommand == null) throw new ArgumentNullException("reloadTrackFileCommand");
            this.saveTrackCommand = saveTrackCommand;
            this.closeWindowCommand = closeWindowCommand;
            this.reloadTrackFileCommand = reloadTrackFileCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new EditTrackViewModel(
                saveTrackCommand, 
                closeWindowCommand, 
                reloadTrackFileCommand, 
                parameter);

            var window = new EditTrackWindow {DataContext = viewModel};
            window.ShowDialog();
        }
    }
}