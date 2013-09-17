using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;
using MixPlanner.IO.MixFiles;

namespace MixPlanner.Commands
{
    public class OpenMixCommand : CommandBase
    {
        readonly IDialogService dialogService;
        readonly IMixReader reader;
        readonly IMessenger messenger;
        readonly IGuardUnsavedChangesService guardService;

        public OpenMixCommand(
            IDialogService dialogService, 
            IMixReader reader,
            IMessenger messenger,
            IGuardUnsavedChangesService guardService)
        {
            if (dialogService == null) throw new ArgumentNullException("dialogService");
            if (reader == null) throw new ArgumentNullException("reader");
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (guardService == null) throw new ArgumentNullException("guardService");
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
                return;

            await reader.ReadAsync(filename)
                  .ContinueWith(m => messenger.Send(new MixLoadedEvent(m.Result)));
        }
    }
}