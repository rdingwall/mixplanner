using System.Collections.Generic;

namespace Julana.CommandLine.DomainModel
{
    public interface INextTrackAdvisor
    {
        IDictionary<Track, IMixingStrategy> GetSuggestionsForNextTrack(Track currentTrack, IEnumerable<Track> unplayedTracks);
    }
}