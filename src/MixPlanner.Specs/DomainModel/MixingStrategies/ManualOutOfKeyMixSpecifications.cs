using System.Collections.Generic;
using Machine.Specifications;
using MixPlanner.App.DomainModel;
using MixPlanner.App.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(ManualOutOfKeyMix))]
    public class ManualOutOfKeyMixSpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(Key.Key9A);
                                        strategy = new ManualOutOfKeyMix();
                                        unplayedTracks = TestMixes.GetRandomMix();
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_anything_we_havent_played_yet_good_luck =
                () => suggestedTracks.ShouldContainOnly(unplayedTracks);
        }
    }
}