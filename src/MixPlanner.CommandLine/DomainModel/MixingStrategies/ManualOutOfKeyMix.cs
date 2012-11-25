using System;
using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualOutOfKeyMix : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            return true;
        }

        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            return unplayedTracks;
        }

        public string Description { get { return "Out of key mix / train wreck!"; } }
    }
}