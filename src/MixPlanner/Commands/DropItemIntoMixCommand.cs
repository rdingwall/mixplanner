using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class DropItemIntoMixCommand : CommandBase<IDropInfo>
    {
        readonly IMix mix;
        readonly IEnumerable<ICommand> commands; 

        public DropItemIntoMixCommand(
            IMix mix,
            AddTrackToMixCommand addTrackCommand,
            ReorderMixTracksCommand reorderTracksCommand,
            ImportFilesIntoMixCommand importFilesCommand)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
            if (addTrackCommand == null) throw new ArgumentNullException("addTrackCommand");
            if (reorderTracksCommand == null) throw new ArgumentNullException("reorderTracksCommand");
            if (importFilesCommand == null) throw new ArgumentNullException("importFilesCommand");
            
            commands = new ICommand[]
                           {
                               addTrackCommand,
                               reorderTracksCommand,
                               importFilesCommand
                           };
        }

        protected override bool CanExecute(IDropInfo parameter)
        {
            return !mix.IsLocked && commands.Any(c => c.CanExecute(parameter));
        }

        protected override void Execute(IDropInfo parameter)
        {
            var command = commands.FirstOrDefault(c => c.CanExecute(parameter));

            if (command != null)
                command.Execute(parameter);
        }
    }
}