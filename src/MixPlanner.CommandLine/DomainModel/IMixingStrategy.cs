using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel
{
    public interface IMixingStrategy
    {
        IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks);
    }
}