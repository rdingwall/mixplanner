namespace MixPlanner.Commands
{
    using System;
    using MixPlanner.DomainModel;
    using MixPlanner.ViewModels;
    using MixPlanner.Views;

    public sealed class QuickEditHarmonicKeyCommand : CommandBase<Track>
    {
        private readonly SaveHarmonicKeyCommand saveHarmonicKeyCommand;
        private readonly CloseWindowCommand closeCommand;

        public QuickEditHarmonicKeyCommand(
            SaveHarmonicKeyCommand saveHarmonicKeyCommand,
            CloseWindowCommand closeCommand)
        {
            if (saveHarmonicKeyCommand == null)
            {
                throw new ArgumentNullException("saveHarmonicKeyCommand");
            }

            if (closeCommand == null)
            {
                throw new ArgumentNullException("closeCommand");
            }

            this.saveHarmonicKeyCommand = saveHarmonicKeyCommand;
            this.closeCommand = closeCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new QuickEditHarmonicKeyViewModel(parameter, saveHarmonicKeyCommand, closeCommand);

            var window = new QuickEditHarmonicKeyWindow { DataContext = viewModel };
            window.ShowDialog();
        }
    }
}