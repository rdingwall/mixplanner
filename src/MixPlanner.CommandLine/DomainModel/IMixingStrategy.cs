using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel
{
    public interface IMixingStrategy
    {
        bool IsCompatible(Track firstTrack, Track secondTrack);
        IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks);
        string Description { get; }
    }
}