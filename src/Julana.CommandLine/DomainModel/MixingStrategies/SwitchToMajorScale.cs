using System;
using System.Collections.Generic;
using System.Linq;

namespace Julana.CommandLine.DomainModel.MixingStrategies
{
    public class SwitchToMajorScale : IMixingStrategy
    {
        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            if (currentTrack.Key.IsMajor())
                return Enumerable.Empty<Track>();

            return unplayedTracks.Where(t => t.Key.Equals(currentTrack.Key.ToMajor()));
        }
    }
}