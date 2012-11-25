using System;
using System.Collections.Generic;

namespace MixPlanner.CommandLine.DomainModel
{
    public class NextTrackAdvisor : INextTrackAdvisor
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public NextTrackAdvisor(IEnumerable<IMixingStrategy> strategies)
        {
            if (strategies == null) throw new ArgumentNullException("strategies");
            this.strategies = strategies;
        }

        public IDictionary<Track, IMixingStrategy> GetSuggestionsForNextTrack(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            var suggestions = new Dictionary<Track, IMixingStrategy>();
            foreach (var strategy in strategies)
            {
                foreach (var track in strategy.NextSuggestedTracks(currentTrack, unplayedTracks))
                {
                    suggestions.Add(track, strategy);
                }
            }

            return suggestions;
        }
    }
}