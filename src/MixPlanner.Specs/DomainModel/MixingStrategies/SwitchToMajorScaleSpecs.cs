using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(SwitchToMajorScale))]
    public class SwitchToMajorScaleSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(HarmonicKey.Key9A);
                                        strategy = new SwitchToMajorScale();
                                        unplayedTracks = new[]
                                                             {
                                                                 TestTracks.Get(HarmonicKey.Key9B), 
                                                                 TestTracks.Get(HarmonicKey.Key4A),
                                                                 TestTracks.Get(HarmonicKey.Key4B),
                                                                 TestTracks.Get(HarmonicKey.Key4A),
                                                                 TestTracks.Get(HarmonicKey.Key9B),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_the_same_pitch_but_major =
                () => suggestedTracks.Select(t => t.Key).ShouldContainOnly(HarmonicKey.Key9B, HarmonicKey.Key9B);
        }
    }
}