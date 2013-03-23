using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;

namespace MixPlanner.Commands
{
    

    public class AutoMixCommand : CommandBase<IEnumerable<MixItem>>
    {
        readonly IMix mix;

        readonly IAutoMixingContextFactory contextFactory;
        readonly IAutoMixingStrategy strategy;


        public AutoMixCommand(
            IMix mix,
            IAutoMixingStrategy strategy,
            IAutoMixingContextFactory contextFactory)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (contextFactory == null) throw new ArgumentNullException("contextFactory");
            this.mix = mix;
            this.strategy = strategy;
            this.contextFactory = contextFactory;
        }

        protected override bool CanExecute(IEnumerable<MixItem> parameter)
        {
            return parameter != null && parameter.Any();

            // todo: only contiguous blocks?
        }

        protected override void Execute(IEnumerable<MixItem> parameter)
        {
            AutoMixingContext context = contextFactory.CreateContext(mix, parameter);
            AutoMixingResult results = strategy.AutoMix(context);

            if (!results.IsSuccess)
                return;

            int startIndex = mix.IndexOf(parameter.First());

            using (mix.DisableRecalcTransitions())
            {
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
}