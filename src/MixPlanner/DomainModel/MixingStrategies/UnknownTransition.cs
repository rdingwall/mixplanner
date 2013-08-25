using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class UnknownTransition : MixingStrategyBase
    {
        public override bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            return first.IsUnknownBpm ||
                   second.IsUnknownBpm ||
                   IsCompatible(first.ActualKey, second.ActualKey);
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            return HarmonicKey.Unknown.Equals(firstKey) ||
                   HarmonicKey.Unknown.Equals(secondKey);
        }

        public override string Description { get { return "Unknown transition (missing key/BPM)"; } }
    }
}