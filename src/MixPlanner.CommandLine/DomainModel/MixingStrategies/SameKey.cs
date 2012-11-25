using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class SameKey : IMixingStrategy
    {
        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            return unplayedTracks.Where(t => t.Key.Equals(currentTrack.Key));
        }
    }
}