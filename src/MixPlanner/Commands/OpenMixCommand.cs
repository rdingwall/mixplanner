using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;
using MixPlanner.IO.MixFiles;

namespace MixPlanner.Commands
{
    public class OpenMixCommand : AsyncCommandBase
    {
        readonly IDialogService dialogService;
        readonly IMixReader reader;
        readonly IMessenger messenger;

        public OpenMixCommand(
            IDialogService dialogService, 
            IMixReader reader,
            IMessenger messenger)
        {
            if (dialogService == null) throw new ArgumentNullException("dialogService");
            if (reader == null) throw new ArgumentNullException("reader");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.dialogService = dialogService;
            this.reader = reader;
            this.messenger = messenger;
        }

        protected async override Task DoExecute(object parameter)
        {
            string filename;
            if (!dialogService.TryOpenMix(out filename))
                return;

            await reader.ReadAsync(filename)
                  .ContinueWith(m => messenger.Send(new MixLoadedEvent(m.Result)));
        }
    }
}