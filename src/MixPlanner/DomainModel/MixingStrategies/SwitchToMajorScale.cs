using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SwitchToMajorScale : IMixingStrategy
    {
        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var firstKey = first.ActualKey;
            var secondKey = second.ActualKey;

            return firstKey.Pitch == secondKey.Pitch
                   && firstKey.IsMinor()
                   && secondKey.IsMajor();
        }

        public string Description { get { return "Switch to major scale"; } }
    }
}