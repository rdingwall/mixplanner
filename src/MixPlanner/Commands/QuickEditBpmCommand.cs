using System;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class QuickEditBpmCommand : CommandBase<Track>
    {
        readonly SaveBpmCommand saveBpmCommand;
        readonly CloseWindowCommand closeCommand;

        public QuickEditBpmCommand(SaveBpmCommand saveBpmCommand,
            CloseWindowCommand closeCommand)
        {
            if (saveBpmCommand == null) throw new ArgumentNullException("saveBpmCommand");
            if (closeCommand == null) throw new ArgumentNullException("closeCommand");
            this.saveBpmCommand = saveBpmCommand;
            this.closeCommand = closeCommand;
        }

        protected override void Execute(Track parameter)
        {
            var viewModel = new QuickEditBpmViewModel(parameter, saveBpmCommand, closeCommand);

            var window = new QuickEditBpmWindow { DataContext = viewModel };
            window.ShowDialog();
        }
    }
}