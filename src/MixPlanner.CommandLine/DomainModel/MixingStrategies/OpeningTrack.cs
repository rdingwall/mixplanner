using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class OpeningTrack : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            return true;
        }

        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            yield break;
        }

        public string Description { get { return "Intro"; } }
    }
}