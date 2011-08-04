using System.Collections.Generic;

namespace Julana.CommandLine.DomainModel
{
    public interface IMixingStrategy
    {
        IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks);
    }
}