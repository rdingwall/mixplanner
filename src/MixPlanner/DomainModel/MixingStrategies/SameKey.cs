using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    // Aka tonic
    public class SameKey : CompatibleBpmMixingStrategyBase
    {
        public SameKey(IBpmRangeChecker bpmRangeChecker)
            : base(bpmRangeChecker)
        {
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            return secondKey.Equals(firstKey);
        }

        public override string Description { get { return "Same key"; } }
    }
}