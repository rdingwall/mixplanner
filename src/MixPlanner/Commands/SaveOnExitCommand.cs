namespace MixPlanner.Commands
{
    using System;
    using System.ComponentModel;

    public sealed class SaveOnExitCommand : CommandBase<CancelEventArgs>
    {
        private readonly IGuardUnsavedChangesService guardService;

        public SaveOnExitCommand(IGuardUnsavedChangesService guardService)
        {
            if (guardService == null)
            {
                throw new ArgumentNullException("guardService");
            }

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