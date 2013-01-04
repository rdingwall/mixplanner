using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SwitchToMinorScale : CompatibleBpmMixingStrategyBase
    {
        public SwitchToMinorScale(IBpmRangeChecker bpmRangeChecker) : base(bpmRangeChecker)
        {
        }

        protected override bool IsCompatibleKey(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var firstKey = first.ActualKey;
            var secondKey = second.ActualKey;

            return firstKey.Pitch == secondKey.Pitch
                   && firstKey.IsMajor()
                   && secondKey.IsMinor();
        }

        public override string Description { get { return "Switch to minor scale"; } }
    }
}