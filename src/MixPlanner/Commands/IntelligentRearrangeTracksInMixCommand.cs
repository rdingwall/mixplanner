using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;

namespace MixPlanner.Commands
{
    public class IntelligentRearrangeTracksInMixCommand : CommandBase<IEnumerable<MixItem>>
    {
        readonly IMix mix;

        readonly IAutoMixingStrategy<MixItem> strategy;

        public IntelligentRearrangeTracksInMixCommand(
            IMix mix, IAutoMixingStrategy<MixItem> strategy)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (strategy == null) throw new ArgumentNullException("strategy");
            this.mix = mix;
            this.strategy = strategy;
        }

        protected override bool CanExecute(IEnumerable<MixItem> parameter)
        {
            return parameter != null && parameter.Any();

            // todo: only contiguous blocks?
        }

        protected override void Execute(IEnumerable<MixItem> parameter)
        {
            var context = new AutoMixingContext<MixItem>(parameter);

            AutoMixingResult<MixItem> results = strategy.AutoMix(context);

            int startIndex = mix.IndexOf(parameter.First());

            using (mix.DisableRecalcTransitions())
            {
                for (int i = 0; i < results.MixedTracks.Count; i++)
                {
                    MixItem item = results.MixedTracks[i];
                    mix.Reorder(item, startIndex + i);
                }

                foreach (MixItem unknownTrack in results.UnknownTracks)
                    mix.MoveToEnd(unknownTrack);
            }
        }
    }
}