using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SwitchToMinorScale : IMixingStrategy
    {
        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var firstKey = first.ActualEndingKey;
            var secondKey = second.ActualStartingKey;

            return firstKey.Pitch == secondKey.Pitch
                   && firstKey.IsMajor()
                   && secondKey.IsMinor();
        }

        public string Description { get { return "Switch to minor scale"; } }
    }
}