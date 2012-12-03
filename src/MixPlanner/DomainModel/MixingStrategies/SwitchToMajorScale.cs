using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SwitchToMajorScale : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            var firstKey = firstTrack.OriginalKey;
            var secondKey = secondTrack.OriginalKey;

            return firstKey.Pitch == secondKey.Pitch
                   && firstTrack.OriginalKey.IsMinor() 
                   && secondTrack.OriginalKey.IsMajor();
        }

        public string Description { get { return "Switch to major scale"; } }
    }
}