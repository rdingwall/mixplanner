using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(OneSemitoneEnergyBoost))]
    public class OneSemitoneEnergyBoostSpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = new Track("A", Key.Key9A);
                                        strategy = new OneSemitoneEnergyBoost();
                                        unplayedTracks = new[]
                                                             {
                                                                 new Track("B", Key.Key8B), 
                                                                 new Track("C", Key.Key4A),
                                                                 new Track("D", Key.Key4B),
                                                                 new Track("E", Key.Key4A),
                                                                 new Track("E", Key.Key8A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_one_semitone_up_from_the_current =
                () => suggestedTracks.Select(t => t.Name).ShouldContainOnly("C", "E");
        }
    }
}