namespace MixPlanner.Commands
{
    using System;
    using MixPlanner.DomainModel;
    using MixPlanner.ViewModels;
    using MixPlanner.Views;

    public sealed class EditTrackCommand : CommandBase<Track>
    {
        private readonly SaveTrackCommand saveTrackCommand;
        private readonly CloseWindowCommand closeWindowCommand;
        private readonly ReloadTrackFileCommand reloadTrackFileCommand;
        private readonly NavigateUriCommand navigateUriCommand;

        public EditTrackCommand(
            SaveTrackCommand saveTrackCommand,
            CloseWindowCommand closeWindowCommand,
            ReloadTrackFileCommand reloadTrackFileCommand,
            NavigateUriCommand navigateUriCommand)
        {
            if (saveTrackCommand == null)
            {
                throw new ArgumentNullException("saveTrackCommand");
            }

            if (closeWindowCommand == null)
            {
                throw new ArgumentNullException("closeWindowCommand");
            }

            if (reloadTrackFileCommand == null)
            {
                throw new ArgumentNullException("reloadTrackFileCommand");
            }

            if (navigateUriCommand == null)
            {
                throw new ArgumentNullException("navigateUriCommand");
            }

            this.saveTrackCommand = saveTrackCommand;
            this.closeWindowCommand = closeWindowCommand;
            this.reloadTrackFileCommand = reloadTrackFileCommand;
            this.navigateUriCommand = navigateUriCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new EditTrackWindowViewModel(
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