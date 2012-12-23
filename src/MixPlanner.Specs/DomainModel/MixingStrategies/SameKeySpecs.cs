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
            Establish context =
                () =>
                    {
                        current = TestPlaybackSpeeds.Starting(HarmonicKey.Key9A, 128);
                        strategy = new SameKey();
                        unplayed = new[]
                                       {
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key8B, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key11A, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key11B, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key11A, 128),
                                           TestPlaybackSpeeds.Starting(HarmonicKey.Key9A, 128)
                                       };
                    };

            static PlaybackSpeed current;
            static IMixingStrategy strategy;
            static PlaybackSpeed[] unplayed;
            static IEnumerable<PlaybackSpeed> suggested;

            Because of = () => suggested = unplayed.Where(t => strategy.IsCompatible(current, t));

            It should_suggest_tracks_that_are_in_the_same_key =
                () => suggested.Select(t => t.ActualStartingKey).ShouldContainOnly(HarmonicKey.Key9A);
        }
    }
}