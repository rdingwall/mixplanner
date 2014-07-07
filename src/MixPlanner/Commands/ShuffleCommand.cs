namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MixPlanner.DomainModel;

    public sealed class ShuffleCommand : SortTracksCommandBase
    {
        // No seed needed here (but two instances of mixplanner with the exact
        // same tracks loaded, shuffled the same number of times would produce
        // two identical mixes!!)
        private readonly Random random;

        public ShuffleCommand(ICurrentMixProvider mixProvider, IDispatcherMessenger messenger) 
            : base(mixProvider, messenger)
        {
            random = new Random();
        }

        protected override bool TrySort(
            IEnumerable<IMixItem> selectedItems, out IEnumerable<IMixItem> sortedItems)
        {
            sortedItems = selectedItems.OrderBy(_ => random.Next());
            return true;
        }
    }
}