using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public abstract class CompatibleBpmMixingStrategyBase : IMixingStrategy
    {
        readonly IBpmRangeChecker bpmRangeChecker;

        protected CompatibleBpmMixingStrategyBase(IBpmRangeChecker bpmRangeChecker)
        {
            if (bpmRangeChecker == null) throw new ArgumentNullException("bpmRangeChecker");
            this.bpmRangeChecker = bpmRangeChecker;
        }

        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return bpmRangeChecker.IsWithinBpmRange(first, second) &&
                   IsCompatibleKey(first, second);
        }

        protected abstract bool IsCompatibleKey(PlaybackSpeed first, PlaybackSpeed second);

        public abstract string Description { get; }

        public bool Equals(IMixingStrategy other)
        {
            return other != null && String.Equals(other.Description, Description);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}