using System.Collections.Generic;
using System.Linq;
using Julana.CommandLine.DomainModel;
using Julana.CommandLine.DomainModel.MixingStrategies;
using Machine.Specifications;

namespace Julana.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(TwoSemitoneEnergyBoost))]
    public class TwoSemitoneEnergyBoostSpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = new Track("A", Key.Key9A);
                                        strategy = new TwoSemitoneEnergyBoost();
                                        unplayedTracks = new[]
                                                             {
                                                                 new Track("B", Key.Key8B), 
                                                                 new Track("C", Key.Key11A),
                                                                 new Track("D", Key.Key11B),
                                                                 new Track("E", Key.Key11A),
                                                                 new Track("E", Key.Key8A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_two_semitone_up_from_the_current =
                () => suggestedTracks.Select(t => t.Name).ShouldContainOnly("C", "E");
        }
    }
}