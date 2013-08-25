using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public abstract class CompatibleBpmMixingStrategyBase : MixingStrategyBase
    {
        readonly IBpmRangeChecker bpmRangeChecker;

        protected CompatibleBpmMixingStrategyBase(IBpmRangeChecker bpmRangeChecker)
        {
            if (bpmRangeChecker == null) throw new ArgumentNullException("bpmRangeChecker");
            this.bpmRangeChecker = bpmRangeChecker;
        }

        public override bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return bpmRangeChecker.IsWithinBpmRange(first, second) &&
                   IsCompatible(first.ActualKey, second.ActualKey);
        }
    }
}