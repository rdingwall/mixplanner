using System;
using System.Collections.Generic;
using System.Linq;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class DragTracksOutOfMixCommand : CommandBase<DropInfo>
    {
        readonly IMix mix;
        readonly RemoveTracksFromMixCommand removeCommand;

        public DragTracksOutOfMixCommand(IMix mix, RemoveTracksFromMixCommand removeCommand)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (removeCommand == null) throw new ArgumentNullException("removeCommand");
            this.mix = mix;
            this.removeCommand = removeCommand;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null)
                return false;

            if (mix.IsLocked)
                return false;

            return parameter.Data is IMixItem || IsCollection(parameter.Data);
        }

        static bool IsCollection(object obj)
        {
            var items = obj as IEnumerable<IMixItem>;
            return items != null && items.Any();
        }

        protected override void Execute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as IMixItem;

            if (sourceItem != null)
                removeCommand.Execute(new[] { sourceItem });

            var sourceItems = parameter.Data as IEnumerable<IMixItem>;

            if (sourceItems != null && sourceItems.Any())
                removeCommand.Execute(sourceItems);
        }
    }
}