using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(IncrementOne))]
    public class IncrementOneSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(Key.Key9A);
                                        strategy = new IncrementOne();
                                        unplayedTracks = new[]
                                                             {
                                                                 TestTracks.Get(Key.Key10A), 
                                                                 TestTracks.Get(Key.Key4A),
                                                                 TestTracks.Get(Key.Key4B),
                                                                 TestTracks.Get(Key.Key4A),
                                                                 TestTracks.Get(Key.Key8A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_one_hour_up_from_the_current =
                () => suggestedTracks.Select(t => t.Key).ShouldContainOnly(Key.Key10A);
        }
    }
}