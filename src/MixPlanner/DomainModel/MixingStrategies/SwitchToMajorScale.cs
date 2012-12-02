using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SwitchToMajorScale : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            var firstKey = firstTrack.Key;
            var secondKey = secondTrack.Key;

            return firstKey.Pitch == secondKey.Pitch
                   && firstTrack.Key.IsMinor() 
                   && secondTrack.Key.IsMajor();
        }

        public string Description { get { return "Switch to major scale"; } }
    }
}