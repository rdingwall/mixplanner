using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(ManualOutOfKeyMix))]
    public class ManualOutOfKeyMixSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(HarmonicKey.Key9A);
                                        strategy = new ManualOutOfKeyMix();
                                        unplayedTracks = TestMixes.GetRandomMix().Tracks;
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = unplayedTracks.Where(t => strategy.IsCompatible(currentTrack, t));
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_anything_we_havent_played_yet_good_luck =
                () => suggestedTracks.ShouldContainOnly(unplayedTracks);
        }
    }
}