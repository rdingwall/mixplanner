using System;
using System.Collections.Generic;
using MixPlanner.DomainModel;
using System.Linq;

namespace MixPlanner.Commands
{
    public class ShuffleCommand : SortTracksCommandBase
    {
        // No seed needed here (but two instances of mixplanner with the exact
        // same tracks loaded, shuffled the same number of times would produce
        // two identical mixes!!)
        private readonly Random random;

        public ShuffleCommand(IMix mix, IDispatcherMessenger messenger) 
            : base(mix, messenger)
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