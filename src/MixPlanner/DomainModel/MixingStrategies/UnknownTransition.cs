using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class UnknownTransition : IMixingStrategy
    {
        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            return first.IsUnknownBpm ||
                   second.IsUnknownBpm ||
                   IsCompatible(first.ActualKey, second.ActualKey);
        }

        public bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            return HarmonicKey.Unknown.Equals(firstKey) ||
                   HarmonicKey.Unknown.Equals(secondKey);
        }

        public string Description { get { return "Unknown transition (missing key/BPM)"; } }

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