using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    /// <summary>
    /// Base for commands that change the order of tracks in the mix according
    /// to some logic e.g. random shuffling or automix.
    /// </summary>
    public abstract class SortTracksCommandBase : AsyncCommandBase<IEnumerable<IMixItem>>
    {
        protected readonly ICurrentMixProvider mixProvider;
        protected readonly IDispatcherMessenger Messenger;

        protected SortTracksCommandBase(ICurrentMixProvider mixProvider, IDispatcherMessenger messenger)
        {
            if (mixProvider == null) throw new ArgumentNullException("mixProvider");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.mixProvider = mixProvider;
            this.Messenger = messenger;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            IMix mix = mixProvider.GetCurrentMix();

            return !mix.IsLocked && parameter != null && parameter.Any();

            // todo: only contiguous blocks?
        }

        protected override async Task DoExecute(IEnumerable<IMixItem> parameter)
        {
            await Task.Factory.StartNew(() => DoExecuteSync(parameter));
        }

        protected abstract bool TrySort(
            IEnumerable<IMixItem> selectedItems, out IEnumerable<IMixItem> sortedItems);

        void DoExecuteSync(IEnumerable<IMixItem> parameter)
        {
            IMix mix = mixProvider.GetCurrentMix();

            using (mix.Lock())
            using (new DisableRecommendationsScope(Messenger))
            {
                IEnumerable<IMixItem> newOrder;
                if (!TrySort(parameter, out newOrder))
                    return;

                ApplyNewOrdering(newOrder, originalOrder: parameter);
            }
        }

        void ApplyNewOrdering(IEnumerable<IMixItem> newOrder, IEnumerable<IMixItem> originalOrder)
        {
            IMix mix = mixProvider.GetCurrentMix();

            // Re-ordered results will be moved to a contiguous block starting
            // at the index of the first track in the original un-mixed
            // selection.
            var newOrderList = newOrder.ToList();
            int startIndex = mix.IndexOf(originalOrder.First());

            for (int i = 0; i < newOrderList.Count; i++)
            {
                IMixItem item = newOrderList[i];
                mix.Reorder(item, startIndex + i);
            }
        } 
    }
}