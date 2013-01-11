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
        readonly NavigateUriCommand navigateUriCommand;

        public EditTrackCommand(
            SaveTrackCommand saveTrackCommand,
            CloseWindowCommand closeWindowCommand,
            ReloadTrackFileCommand reloadTrackFileCommand,
            NavigateUriCommand navigateUriCommand)
        {
            if (saveTrackCommand == null) throw new ArgumentNullException("saveTrackCommand");
            if (closeWindowCommand == null) throw new ArgumentNullException("closeWindowCommand");
            if (reloadTrackFileCommand == null) throw new ArgumentNullException("reloadTrackFileCommand");
            if (navigateUriCommand == null) throw new ArgumentNullException("navigateUriCommand");
            this.saveTrackCommand = saveTrackCommand;
            this.closeWindowCommand = closeWindowCommand;
            this.reloadTrackFileCommand = reloadTrackFileCommand;
            this.navigateUriCommand = navigateUriCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new EditTrackViewModel(
                saveTrackCommand, 
                closeWindowCommand, 
                reloadTrackFileCommand, 
                navigateUriCommand,
                parameter);

            var window = new EditTrackWindow {DataContext = viewModel};
            window.ShowDialog();
        }
    }
}