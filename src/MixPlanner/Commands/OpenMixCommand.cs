namespace MixPlanner.Commands
{
    using System;
    using GalaSoft.MvvmLight.Messaging;
    using MixPlanner.Events;
    using MixPlanner.IO.MixFiles;

    public sealed class OpenMixCommand : CommandBase
    {
        private readonly IDialogService dialogService;
        private readonly IMixReader reader;
        private readonly IMessenger messenger;
        private readonly IGuardUnsavedChangesService guardService;

        public OpenMixCommand(
            IDialogService dialogService, 
            IMixReader reader,
            IMessenger messenger,
            IGuardUnsavedChangesService guardService)
        {
            if (dialogService == null)
            {
                throw new ArgumentNullException("dialogService");
            }

            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }

            if (guardService == null)
            {
                throw new ArgumentNullException("guardService");
            }

            this.dialogService = dialogService;
            this.reader = reader;
            this.messenger = messenger;
            this.guardService = guardService;
        }

        public override void Execute(object parameter)
        {
            guardService.GuardUnsavedChanges(DoOpenMix);
        }

        public async void DoOpenMix()
        {
            string filename;
            if (!dialogService.TryOpenMix(out filename))
            {
                return;
            }

            await reader.ReadAsync(filename)
                  .ContinueWith(m => messenger.Send(new MixLoadedEvent(m.Result)));
        }
    }
}