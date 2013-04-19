using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class AutoMixCommand : AsyncCommandBase<IEnumerable<IMixItem>>
    {
        readonly IMix mix;
        readonly IAutoMixingContextFactory contextFactory;
        readonly IDispatcherMessenger messenger;
        readonly IAutoMixingStrategy strategy;

        public AutoMixCommand(
            IMix mix,
            IAutoMixingStrategy strategy,
            IAutoMixingContextFactory contextFactory,
            IDispatcherMessenger messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (contextFactory == null) throw new ArgumentNullException("contextFactory");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.mix = mix;
            this.strategy = strategy;
            this.contextFactory = contextFactory;
            this.messenger = messenger;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            return !mix.IsLocked && parameter != null && parameter.Any();

            // todo: only contiguous blocks?
        }

        protected override async Task DoExecute(IEnumerable<IMixItem> parameter)
        {
            await Task.Factory.StartNew(() => DoExecuteSync(parameter));
        }

        void DoExecuteSync(IEnumerable<IMixItem> parameter)
        {
            using (mix.Lock())
            using (new DisableRecommendationsScope(messenger))
            {
                messenger.SendToUI(new BeganAutoMixingEvent());
                
                mix.AutoAdjustBpms(parameter);
                AutoMixingContext context = contextFactory.CreateContext(mix, parameter);
                AutoMixingResult results = strategy.AutoMix(context);

                if (!results.IsSuccess)
                    return;

                ApplyNewOrdering(results, originalOrder: parameter);
            }
        }

        void ApplyNewOrdering(AutoMixingResult results, IEnumerable<IMixItem> originalOrder)
        {
            // Automixed results will be moved to a contiguous block starting
            // at the index of the first track in the original un-mixed
            // selection.
            int startIndex = mix.IndexOf(originalOrder.First());

            for (int i = 0; i < results.MixedTracks.Count; i++)
            {
                IMixItem item = results.MixedTracks[i];
                mix.Reorder(item, startIndex + i);
            }

            foreach (IMixItem unknownTrack in results.UnknownTracks)
                mix.MoveToEnd(unknownTrack);
        }
    }
}