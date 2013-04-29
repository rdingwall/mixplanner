using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class AutoMixCommand : ChangeTrackOrderCommandBase
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

        protected override bool TryCalculateNewOrderOfTracks(
            IEnumerable<IMixItem> selectedItems,
            out IEnumerable<IMixItem> newOrder)
        {
            Messenger.SendToUI(new BeganAutoMixingEvent());

            Mix.AutoAdjustBpms(selectedItems);
            AutoMixingContext context = contextFactory.CreateContext(Mix, selectedItems);
            AutoMixingResult results = strategy.AutoMix(context);

            if (!results.IsSuccess)
            {
                newOrder = null;
                return false;
            }
            
            newOrder = results.MixedTracks.Concat(results.UnknownTracks);
            return true;
        }
    }
}