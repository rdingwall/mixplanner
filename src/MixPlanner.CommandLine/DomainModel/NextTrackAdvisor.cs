using System;
using System.Collections.Generic;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.CommandLine.DomainModel
{
    public class NextTrackAdvisor : INextTrackAdvisor
    {
        public NextTrackAdvisor()
        {
            PreferredStrategies = new List<IMixingStrategy>
                                      {
                                          new TwoSemitoneEnergyBoost(),
                                          new SameKey(),
                                          new OneSemitoneEnergyBoost(),
                                          new IncrementOne(),
                                          new SwitchToMajorScale(),
                                          new SwitchToMinorScale()
                                      };
        }

        public IDictionary<Track, IMixingStrategy> GetSuggestionsForNextTrack(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            var suggestions = new Dictionary<Track, IMixingStrategy>();
            foreach (var strategy in PreferredStrategies)
            {
                foreach (var track in strategy.NextSuggestedTracks(currentTrack, unplayedTracks))
                {
                    suggestions.Add(track, strategy);
                }
            }

            return suggestions;
        }

        public IList<IMixingStrategy> PreferredStrategies { get; private set; }
    }
}