using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel
{
    public interface INextTrackAdvisor
    {
        IDictionary<Track, IMixingStrategy> GetSuggestionsForNextTrack(Track currentTrack, IEnumerable<Track> unplayedTracks);
    }
}