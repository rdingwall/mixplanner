using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class RelativeMinor : CompatibleBpmMixingStrategyBase
    {
        public RelativeMinor(IBpmRangeChecker bpmRangeChecker) : base(bpmRangeChecker)
        {
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            return firstKey.Pitch == secondKey.Pitch
                   && firstKey.IsMajor()
                   && secondKey.IsMinor();
        }

        public override string Description { get { return "Relative Minor (B -> A)"; } }
    }
}