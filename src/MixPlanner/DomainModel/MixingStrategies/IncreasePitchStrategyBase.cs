using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public abstract class IncreasePitchStrategyBase : CompatibleBpmMixingStrategyBase
    {
        readonly int increaseAmount;

        protected IncreasePitchStrategyBase(IBpmRangeChecker bpmRangeChecker, int increaseAmount)
            : base(bpmRangeChecker)
        {
            this.increaseAmount = increaseAmount;
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            if (firstKey == null) throw new ArgumentNullException("firstKey");
            if (secondKey == null) throw new ArgumentNullException("secondKey");

            return secondKey.HasSameScaleAs(firstKey)
                   && secondKey.Equals(firstKey.Advance(increaseAmount));
        }
    }
}