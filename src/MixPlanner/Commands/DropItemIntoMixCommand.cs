namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using GongSolutions.Wpf.DragDrop;
    using MixPlanner.DomainModel;

    public sealed class DropItemIntoMixCommand : CommandBase<IDropInfo>
    {
        private readonly ICurrentMixProvider mixProvider;
        private readonly IEnumerable<ICommand> commands; 

        public DropItemIntoMixCommand(
            ICurrentMixProvider mixProvider,
            AddTrackToMixCommand addTrackCommand,
            ReorderMixTracksCommand reorderTracksCommand,
            ImportFilesIntoMixCommand importFilesCommand)
        {
            if (mixProvider == null)
            {
                throw new ArgumentNullException("mixProvider");
            }
            
            if (addTrackCommand == null)
            {
                throw new ArgumentNullException("addTrackCommand");
            }

            if (reorderTracksCommand == null)
            {
                throw new ArgumentNullException("reorderTracksCommand");
            }

            if (importFilesCommand == null)
            {
                throw new ArgumentNullException("importFilesCommand");
            }

            this.mixProvider = mixProvider;

            commands = new ICommand[]
                           {
                               addTrackCommand,
                               reorderTracksCommand,
                               importFilesCommand
                           };
        }

        protected override bool CanExecute(IDropInfo parameter)
        {
            IMix mix = mixProvider.GetCurrentMix();

            return !mix.IsLocked && commands.Any(c => c.CanExecute(parameter));
        }

        protected override void Execute(IDropInfo parameter)
        {
            var command = commands.FirstOrDefault(c => c.CanExecute(parameter));

            if (command != null)
            {
                command.Execute(parameter);
            }
        }
    }
}