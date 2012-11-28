using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(SameKey))]
    public class SameKeySpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(HarmonicKey.Key9A);
                                        strategy = new SameKey();
                                        unplayedTracks = new[]
                                                             {
                                                                 TestTracks.Get(HarmonicKey.Key8B), 
                                                                 TestTracks.Get(HarmonicKey.Key11A),
                                                                 TestTracks.Get(HarmonicKey.Key11B),
                                                                 TestTracks.Get(HarmonicKey.Key11A),
                                                                 TestTracks.Get(HarmonicKey.Key9A)
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_in_the_same_key =
                () => suggestedTracks.Select(t => t.Key).ShouldContainOnly(HarmonicKey.Key9A);
        }
    }
}