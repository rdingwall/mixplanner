using System.Collections.Generic;

namespace Julana.CommandLine.DomainModel.MixingStrategies
{
    public class OpeningTrack : IMixingStrategy
    {
        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            yield break;
        }
    }
}