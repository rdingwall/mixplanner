namespace MixPlanner.Commands
{
    using System;
    using System.Linq;
    using GongSolutions.Wpf.DragDrop;
    using MixPlanner.DomainModel;

    public sealed class ReorderMixTracksCommand : CommandBase<IDropInfo>
    {
        private readonly ICurrentMixProvider mixProvider;

        public ReorderMixTracksCommand(ICurrentMixProvider mixProvider)
        {
            if (mixProvider == null)
            {
                throw new ArgumentNullException("mixProvider");
            }

            this.mixProvider = mixProvider;
        }

        protected override bool CanExecute(IDropInfo parameter)
        {
            IMix mix = mixProvider.GetCurrentMix();

            return !mix.IsLocked && parameter != null
                   && parameter.DataAsEnumerable<IMixItem>().Any();
        }

        protected override void Execute(IDropInfo parameter)
        {
            var items = parameter.DataAsEnumerable<IMixItem>().ToList();

            IMix mix = mixProvider.GetCurrentMix();

            for (int i = 0; i < items.Count; i++)
            {
                mix.Reorder(items[i], parameter.InsertIndex + i);
            }
        }
    }
}