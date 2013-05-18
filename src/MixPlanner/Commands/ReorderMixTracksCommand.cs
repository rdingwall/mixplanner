using System;
using System.Linq;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class ReorderMixTracksCommand : CommandBase<IDropInfo>
    {
        readonly IMix mix;

        public ReorderMixTracksCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool CanExecute(IDropInfo parameter)
        {
            return !mix.IsLocked && parameter != null
                   && parameter.DataAsEnumerable<IMixItem>().Any();
        }

        protected override void Execute(IDropInfo parameter)
        {
            var items = parameter.DataAsEnumerable<IMixItem>().ToList();

            for (int i = 0; i < items.Count; i++)
                mix.Reorder(items[i], parameter.InsertIndex + i);
        }
    }
}