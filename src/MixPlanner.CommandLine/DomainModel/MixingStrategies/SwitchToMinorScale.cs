using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class SwitchToMinorScale : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            if (firstTrack.Key.IsMinor())
                return false; // already in minor scale

            return secondTrack.Key.IsMinor();
        }

        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");
            
            if (currentTrack.Key.IsMinor())
                return Enumerable.Empty<Track>();

            return unplayedTracks.Where(t => t.Key.Equals(currentTrack.Key.ToMinor()));
        }

        public string Description { get { return "Switch to minor scale"; } }
    }
}