using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(UnknownTransition))]
    public class UnknownTransitionSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context =
                () =>
                    {
                        current = new PlaybackSpeed(HarmonicKey.Key6A, 128);
                        strategy = new UnknownTransition();
                        unplayed = new[]
                                       {
                                           new PlaybackSpeed(HarmonicKey.Key6A, 128),
                                           new PlaybackSpeed(HarmonicKey.Unknown, Double.NaN),
                                           new PlaybackSpeed(HarmonicKey.Key6B, Double.NaN),
                                           new PlaybackSpeed(HarmonicKey.Unknown, 127) 
                                       };
                    };

            static PlaybackSpeed current;
            static IMixingStrategy strategy;
            static IList<PlaybackSpeed> unplayed;
            static IEnumerable<PlaybackSpeed> suggested;

            Because of = () => suggested = unplayed.Where(t => strategy.IsCompatible(current, t));

            It should_only_recommend_tracks_with_incomplete_information = 
                () => suggested.ShouldContainOnly(unplayed[1], unplayed[2], unplayed[3]);
        }
    }
}