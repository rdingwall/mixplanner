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

        protected override bool IsCompatibleKey(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return second.ActualKey.HasSameScaleAs(first.ActualKey)
                   && second.ActualKey.Equals(first.ActualKey.IncreasePitch(increaseAmount));
        }

        public override string Description
        {
            get
            {
                return String.Format("Increase pitch by {0}", increaseAmount);
            }
        }
    }
}