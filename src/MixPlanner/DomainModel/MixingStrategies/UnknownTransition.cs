using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class UnknownTransition : IMixingStrategy
    {
        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            return first.IsUnknownBpm ||
                   second.IsUnknownBpm ||
                   HarmonicKey.Unknown.Equals(first.ActualKey) ||
                   HarmonicKey.Unknown.Equals(second.ActualKey);
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