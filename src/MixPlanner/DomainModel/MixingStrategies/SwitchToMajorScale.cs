using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public class SwitchToMajorScale : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            if (firstTrack.Key.IsMajor())
                return false; // already in major scale

            return secondTrack.Key.IsMajor();
        }

        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            if (currentTrack.Key.IsMajor())
                return Enumerable.Empty<Track>();

            return unplayedTracks.Where(t => t.Key.Equals(currentTrack.Key.ToMajor()));
        }

        public string Description { get { return "Switch to major scale"; } }
    }
}