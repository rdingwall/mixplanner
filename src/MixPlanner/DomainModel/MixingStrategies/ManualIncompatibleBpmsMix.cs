using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualIncompatibleBpmsMix : IMixingStrategy
    {
        readonly IBpmRangeChecker bpmRangeChecker;

        public ManualIncompatibleBpmsMix(IBpmRangeChecker bpmRangeChecker)
        {
            if (bpmRangeChecker == null) throw new ArgumentNullException("bpmRangeChecker");
            this.bpmRangeChecker = bpmRangeChecker;
        }

        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            return !bpmRangeChecker.IsWithinBpmRange(first, second);
        }

        public string Description { get { return "BPMs out of range / train wreck!"; } }
    }
}