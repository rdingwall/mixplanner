using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(SwitchToMinorScale))]
    public class SwitchToMinorScaleSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(HarmonicKey.Key9B);
                                        strategy = new SwitchToMinorScale();
                                        unplayedTracks = new[]
                                                             {
                                                                 TestTracks.Get(HarmonicKey.Key9A), 
                                                                 TestTracks.Get(HarmonicKey.Key4A),
                                                                 TestTracks.Get(HarmonicKey.Key4B),
                                                                 TestTracks.Get(HarmonicKey.Key4A),
                                                                 TestTracks.Get(HarmonicKey.Key9A)
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = unplayedTracks.Where(t => strategy.IsCompatible(currentTrack, t));
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_the_same_pitch_but_minor =
                () => suggestedTracks.Select(t => t.Key).Distinct().ShouldContainOnly(HarmonicKey.Key9A);
        }
    }
}