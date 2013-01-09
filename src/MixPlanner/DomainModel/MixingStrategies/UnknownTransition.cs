using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class UnknownTransition : IMixingStrategy
    {
        public bool Equals(IMixingStrategy other)
        {
            return String.Equals(Description, other.Description);
        }

        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
        }

        public string Description { get { return "Unknown transition"; } }}
    }
}