namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using MixPlanner.DomainModel;
    using MixPlanner.Events;
    using MixPlanner.IO.MixFiles;

    public sealed class SaveMixCommand : AsyncCommandBase<IMix>
    {
        private readonly SaveMixAsCommand saveAsCommand;
        private readonly IMixWriter writer;
        private readonly IDispatcherMessenger messenger;

        public SaveMixCommand(
            SaveMixAsCommand saveAsCommand, 
            IMixWriter writer,
            IDispatcherMessenger messenger)
        {
            if (saveAsCommand == null)
            {
                throw new ArgumentNullException("saveAsCommand");
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }

            this.saveAsCommand = saveAsCommand;
            this.writer = writer;
            this.messenger = messenger;
        }

        protected override bool CanExecute(IMix parameter)
        {
            return parameter != null && !parameter.IsEmpty;
        }

        protected async override Task DoExecute(IMix parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.Filename))
            {
                await saveAsCommand.ExecuteAsync(parameter);
            }
            else
            {
                await
                    writer.WriteAsync(parameter, parameter.Filename)
                          .ContinueWith(_ => messenger.SendToUI(new MixSavedEvent(parameter)));
            }
        }
    }
}