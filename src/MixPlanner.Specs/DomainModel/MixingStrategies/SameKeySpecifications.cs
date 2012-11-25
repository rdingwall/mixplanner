using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(SameKey))]
    public class SameKeySpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = new Track("A", Key.Key9A);
                                        strategy = new SameKey();
                                        unplayedTracks = new[]
                                                             {
                                                                 new Track("B", Key.Key8B), 
                                                                 new Track("C", Key.Key11A),
                                                                 new Track("D", Key.Key11B),
                                                                 new Track("E", Key.Key11A),
                                                                 new Track("E", Key.Key9A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_in_the_same_key =
                () => suggestedTracks.Select(t => t.Name).ShouldContainOnly("E");
        }
    }
}