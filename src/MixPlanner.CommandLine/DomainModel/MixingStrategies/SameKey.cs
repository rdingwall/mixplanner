using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class SameKey : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            return secondTrack.Key.Equals(firstTrack.Key);
        }

        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            return unplayedTracks.Where(t => IsCompatible(currentTrack, t));
        }

        public string Description { get { return "Same key"; } }
    }
}