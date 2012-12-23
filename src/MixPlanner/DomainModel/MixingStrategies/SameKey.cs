using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SameKey : IMixingStrategy
    {
        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return second.ActualKey.Equals(first.ActualKey);
        }

        public string Description { get { return "Same key"; } }
    }
}