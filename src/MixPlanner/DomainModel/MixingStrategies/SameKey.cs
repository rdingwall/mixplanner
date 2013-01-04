using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SameKey : CompatibleBpmMixingStrategyBase
    {
        public SameKey(IBpmRangeChecker bpmRangeChecker)
            : base(bpmRangeChecker)
        {
        }

        protected override bool IsCompatibleKey(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return second.ActualKey.Equals(first.ActualKey);
        }

        public override string Description { get { return "Same key"; } }
    }
}