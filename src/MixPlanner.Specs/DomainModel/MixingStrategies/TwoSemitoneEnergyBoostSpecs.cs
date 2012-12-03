﻿using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(TwoSemitoneEnergyBoost))]
    public class TwoSemitoneEnergyBoostSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(HarmonicKey.Key9A);
                                        strategy = new TwoSemitoneEnergyBoost();
                                        unplayedTracks = new[]
                                                             {
                                                                 TestTracks.Get(HarmonicKey.Key8B), 
                                                                 TestTracks.Get(HarmonicKey.Key11A),
                                                                 TestTracks.Get(HarmonicKey.Key11B),
                                                                 TestTracks.Get(HarmonicKey.Key11A),
                                                                 TestTracks.Get(HarmonicKey.Key8A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = unplayedTracks.Where(t => strategy.IsCompatible(currentTrack, t));
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_two_semitone_up_from_the_current =
                () => suggestedTracks.Select(t => t.OriginalKey).ShouldContainOnly(HarmonicKey.Key11A, HarmonicKey.Key11A);
        }
    }
}