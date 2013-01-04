﻿using System.Collections.Generic;
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
                        current = new PlaybackSpeed(HarmonicKey.Key9A, 128);
                        strategy = new SameKey(new AlwaysInRangeBpmChecker());
                        unplayed = new[]
                                       {
                                           new PlaybackSpeed(HarmonicKey.Key8B, 128),
                                           new PlaybackSpeed(HarmonicKey.Key11A, 128),
                                           new PlaybackSpeed(HarmonicKey.Key11B, 128),
                                           new PlaybackSpeed(HarmonicKey.Key11A, 128),
                                           new PlaybackSpeed(HarmonicKey.Key9A, 128)
                                       };
                    };

            static PlaybackSpeed current;
            static IMixingStrategy strategy;
            static PlaybackSpeed[] unplayed;
            static IEnumerable<PlaybackSpeed> suggested;

            Because of = () => suggested = unplayed.Where(t => strategy.IsCompatible(current, t));

            It should_suggest_tracks_that_are_in_the_same_key =
                () => suggested.Select(t => t.ActualKey).ShouldContainOnly(HarmonicKey.Key9A);
        }
    }
}