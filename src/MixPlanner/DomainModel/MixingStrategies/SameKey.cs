using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SameKey : IMixingStrategy
    {
        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return second.ActualStartingKey.Equals(first.ActualStartingKey);
        }

        public string Description { get { return "Same key"; } }
    }
}