using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class RelativeMajor : CompatibleBpmMixingStrategyBase
    {
        public RelativeMajor(IBpmRangeChecker bpmRangeChecker) : base(bpmRangeChecker)
        {
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            if (firstKey == null) throw new ArgumentNullException("firstKey");
            if (secondKey == null) throw new ArgumentNullException("secondKey");

            return firstKey.Pitch == secondKey.Pitch
                   && firstKey.IsMinor()
                   && secondKey.IsMajor();
        }

        public override string Description { get { return "Relative Major (A -> B)"; } }
    }
}