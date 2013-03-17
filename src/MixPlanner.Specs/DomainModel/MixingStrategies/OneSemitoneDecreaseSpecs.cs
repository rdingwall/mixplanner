using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(OneSemitoneDecrease))]
    public class OneSemitoneDecreaseSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context =
                () =>
                    {
                        current = new PlaybackSpeed(HarmonicKey.Key6A, 128);
                        strategy = new OneSemitoneDecrease(new AlwaysInRangeBpmChecker());
                        unplayed = new[]
                                       {
                                           new PlaybackSpeed(HarmonicKey.Key8B, 128),
                                           new PlaybackSpeed(HarmonicKey.Key11A, 128),
                                           new PlaybackSpeed(HarmonicKey.Key11B, 128),
                                           new PlaybackSpeed(HarmonicKey.Key11A, 128),
                                           new PlaybackSpeed(HarmonicKey.Key8A, 128),
                                       };
                    };

            static PlaybackSpeed current;
            static IMixingStrategy strategy;
            static PlaybackSpeed[] unplayed;
            static IEnumerable<PlaybackSpeed> suggested;

            Because of = () => suggested = unplayed.Where(t => strategy.IsCompatible(current, t));

            It should_suggest_tracks_that_are_one_semitone_down_from_the_current =
                () => suggested.Select(t => t.ActualKey).ShouldContainOnly(HarmonicKey.Key11A, HarmonicKey.Key11A);
        }
    }
}