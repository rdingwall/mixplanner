using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(SwitchToMinorScale))]
    public class SwitchToMinorScaleSpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = new Track("A", Key.Key9B);
                                        strategy = new SwitchToMinorScale();
                                        unplayedTracks = new[]
                                                             {
                                                                 new Track("B", Key.Key9A), 
                                                                 new Track("C", Key.Key4A),
                                                                 new Track("D", Key.Key4B),
                                                                 new Track("E", Key.Key4A),
                                                                 new Track("E", Key.Key9A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_the_same_pitch_but_minor =
                () => suggestedTracks.Select(t => t.Name).ShouldContainOnly("B", "E");
        }
    }
}