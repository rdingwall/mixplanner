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

        public bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            throw new InvalidOperationException("This strategy is only used to compare BPMs.");
        }

        public string Description { get { return "BPMs out of range / train wreck!"; } }

        public bool Equals(IMixingStrategy other)
        {
            return other != null && String.Equals(other.Description, Description);
        }
    }
}