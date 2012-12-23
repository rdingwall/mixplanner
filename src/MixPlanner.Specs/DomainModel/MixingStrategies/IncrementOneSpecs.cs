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
            Establish context =
                () =>
                    {
                        current = TestPlaybackSpeeds.Ending(HarmonicKey.Key9A, 128);
                        strategy = new IncrementOne();
                        unplayed = new[]
                                       {
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key10A, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key4A, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key4B, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key4A, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key8A, 128),
                                       };
                    };

            static PlaybackSpeed current;
            static IMixingStrategy strategy;
            static PlaybackSpeed[] unplayed;
            static IEnumerable<PlaybackSpeed> suggested;

            Because of = () => suggested = unplayed.Where(t => strategy.IsCompatible(current, t));

            It should_suggest_tracks_that_are_one_hour_up_from_the_current =
                () => suggested.Select(t => t.ActualStartingKey).ShouldContainOnly(HarmonicKey.Key10A);
        }
    }
}