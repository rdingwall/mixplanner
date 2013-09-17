using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.IO.MixFiles;

namespace MixPlanner.Commands
{
    public class SaveMixAsCommand : AsyncCommandBase<IMix>
    {
        readonly IDialogService dialogService;
        readonly IMixWriter writer;

        public SaveMixAsCommand(IDialogService dialogService, IMixWriter writer)
        {
            if (dialogService == null) throw new ArgumentNullException("dialogService");
            if (writer == null) throw new ArgumentNullException("writer");
            this.dialogService = dialogService;
            this.writer = writer;
        }

        protected override bool CanExecute(IMix parameter)
        {
            return parameter != null && !parameter.IsEmpty;
        }

        protected async override Task DoExecute(IMix parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            string filename;
            if (!dialogService.TrySaveMix(out filename))
                return;

            await writer.WriteAsync(parameter, filename);
        }
    }
}