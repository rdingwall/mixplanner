using System.Collections.Generic;

namespace MixPlanner.DomainModel
{
    public interface INextTrackAdvisor
    {
        IDictionary<Track, IMixingStrategy> GetSuggestionsForNextTrack(Track currentTrack, IEnumerable<Track> unplayedTracks);
    }
}