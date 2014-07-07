namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MixPlanner.DomainModel;
    using MixPlanner.DomainModel.AutoMixing;
    using MixPlanner.Events;

    public sealed class AutoMixCommand : SortTracksCommandBase
    {
        private readonly IAutoMixingContextFactory contextFactory;
        private readonly IAutoMixingStrategy strategy;

        public AutoMixCommand(
            ICurrentMixProvider mixProvider, 
            IDispatcherMessenger messenger,
            IAutoMixingContextFactory contextFactory,
            IAutoMixingStrategy strategy) : base(mixProvider, messenger)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }

            if (strategy == null)
            {
                throw new ArgumentNullException("strategy");
            }

            this.contextFactory = contextFactory;
            this.strategy = strategy;
        }

        protected override bool TrySort(
            IEnumerable<IMixItem> selectedItems,
            out IEnumerable<IMixItem> sortedItems)
        {
            Messenger.SendToUI(new BeganAutoMixingEvent());

            IMix mix = this.MixProvider.GetCurrentMix();

            mix.AutoAdjustBpms(selectedItems);
            AutoMixingContext context = contextFactory.CreateContext(mix, selectedItems);
            AutoMixingResult results = strategy.AutoMix(context);

            if (!results.IsSuccess)
            {
                sortedItems = null;
                return false;
            }
            
            sortedItems = results.MixedTracks.Concat(results.UnknownTracks);
            return true;
        }
    }
}