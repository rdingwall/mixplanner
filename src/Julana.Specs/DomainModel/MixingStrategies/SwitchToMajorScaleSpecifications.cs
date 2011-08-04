using System.Collections.Generic;
using System.Linq;
using Julana.CommandLine.DomainModel;
using Julana.CommandLine.DomainModel.MixingStrategies;
using Machine.Specifications;

namespace Julana.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(SwitchToMajorScale))]
    public class SwitchToMajorScaleSpecifications
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = new Track("A", Key.Key9A);
                                        strategy = new SwitchToMajorScale();
                                        unplayedTracks = new[]
                                                             {
                                                                 new Track("B", Key.Key9B), 
                                                                 new Track("C", Key.Key4A),
                                                                 new Track("D", Key.Key4B),
                                                                 new Track("E", Key.Key4A),
                                                                 new Track("E", Key.Key9B),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = strategy.NextSuggestedTracks(currentTrack, unplayedTracks);
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_the_same_pitch_but_major =
                () => suggestedTracks.Select(t => t.Name).ShouldContainOnly("B", "E");
        }
    }
}