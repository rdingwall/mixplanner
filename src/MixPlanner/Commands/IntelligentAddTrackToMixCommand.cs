using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class IntelligentAddTrackToMixCommand : CommandBase<Track>
    {
        readonly IIntelligentInserter inserter;
        readonly IMix mix;

        public IntelligentAddTrackToMixCommand(
            IIntelligentInserter inserter, IMix mix)
        {
            if (inserter == null) throw new ArgumentNullException("inserter");
            if (mix == null) throw new ArgumentNullException("mix");
            this.inserter = inserter;
            this.mix = mix;
        }

        protected override void Execute(Track parameter)
        {
            var result = inserter.GetBestInsertIndex(mix, parameter);
            if (!result.IsSuccess)
                return;

            // Adjustment will actually be figured out by the mix
            // Should probably refactor this to allow inserting a track at
            // a specific speed.
            mix.Insert(parameter, result.InsertIndex);
        }
    }
}