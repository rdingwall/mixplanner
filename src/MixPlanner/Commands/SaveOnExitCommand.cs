using System;
using System.ComponentModel;

namespace MixPlanner.Commands
{
    public class SaveOnExitCommand : CommandBase<CancelEventArgs>
    {
        readonly IGuardUnsavedChangesService guardService;

        public SaveOnExitCommand(IGuardUnsavedChangesService guardService)
        {
            if (guardService == null) throw new ArgumentNullException("guardService");
            this.guardService = guardService;
        }

        protected override void Execute(CancelEventArgs parameter)
        {
            guardService.GuardUnsavedChanges(
                action: delegate { }, 
                cancel: () => parameter.Cancel = true);
        }
    }
}