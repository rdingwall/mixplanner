using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(ManualIncompatibleBpmsMix))]
    public class ManualIncompatibleBpmsMixSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context =
                () =>
                    {
                        current = new PlaybackSpeed(HarmonicKey.RandomKey(), 128);
                        strategy = new ManualIncompatibleBpmsMix(new AlwaysInRangeBpmChecker());
                        unplayed = TestMixes.GetRandomMix().Items.Select(i => i.PlaybackSpeed);
                    };

            static PlaybackSpeed current;
            static IMixingStrategy strategy;
            static IEnumerable<PlaybackSpeed> unplayed;
            static IEnumerable<PlaybackSpeed> suggested;

            Because of = () => suggested = unplayed.Where(t => strategy.IsCompatible(current, t));

            It should_suggest_anything_we_havent_played_yet_good_luck =
                () => suggested.ShouldBeEmpty();
        }
    }
}