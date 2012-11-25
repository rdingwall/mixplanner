using System;
using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualOutOfKeyMix : IMixingStrategy
    {
        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            return unplayedTracks;
        }
    }
}