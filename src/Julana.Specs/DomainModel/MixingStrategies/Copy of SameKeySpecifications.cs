﻿using System.Collections.Generic;
using System.Linq;
using Julana.CommandLine.DomainModel;
using Julana.CommandLine.DomainModel.MixingStrategies;
using Machine.Specifications;

namespace Julana.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(ManualOutOfKeyMix))]
    public class ManualOutOfKeyMixSpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = new Track("A", Key.Key9A);
                                        strategy = new ManualOutOfKeyMix();
                                        unplayedTracks = new[]
                                                             {
                                                                 new Track("B", Key.Key8B), 
                                                                 new Track("C", Key.Key11A),
                                                                 new Track("D", Key.Key11B),
                                                                 new Track("E", Key.Key11A),
                                                                 new Track("E", Key.Key9A),
                                                             };
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