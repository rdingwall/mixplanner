using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class AutoMixCommand : SortTracksCommandBase
    {
        readonly IAutoMixingContextFactory contextFactory;
        readonly IAutoMixingStrategy strategy;

        public AutoMixCommand(
            IMix mix, 
            IDispatcherMessenger messenger,
            IAutoMixingContextFactory contextFactory,
            IAutoMixingStrategy strategy) : base(mix, messenger)
        {
            if (contextFactory == null) throw new ArgumentNullException("contextFactory");
            if (strategy == null) throw new ArgumentNullException("strategy");
            this.contextFactory = contextFactory;
            this.strategy = strategy;
        }

        protected override bool TrySort(
            IEnumerable<IMixItem> selectedItems,
            out IEnumerable<IMixItem> sortedItems)
        {
            Messenger.SendToUI(new BeganAutoMixingEvent());

            Mix.AutoAdjustBpms(selectedItems);
            AutoMixingContext context = contextFactory.CreateContext(Mix, selectedItems);
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