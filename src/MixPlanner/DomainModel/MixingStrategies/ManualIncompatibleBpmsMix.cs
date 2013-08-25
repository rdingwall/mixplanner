using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualIncompatibleBpmsMix : MixingStrategyBase
    {
        readonly IBpmRangeChecker bpmRangeChecker;

        public ManualIncompatibleBpmsMix(IBpmRangeChecker bpmRangeChecker)
        {
            if (bpmRangeChecker == null) throw new ArgumentNullException("bpmRangeChecker");
            this.bpmRangeChecker = bpmRangeChecker;
        }

        public override bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            return !bpmRangeChecker.IsWithinBpmRange(first, second);
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            throw new InvalidOperationException("This strategy is only used to compare BPMs.");
        }

        public override string Description { get { return "BPMs out of range / train wreck!"; } }
    }
}