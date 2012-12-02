using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel.MixingStrategies
{
    [Subject(typeof(OneSemitoneEnergyBoost))]
    public class OneSemitoneEnergyBoostSpecs
    {
        public class when_deciding_which_track_to_play_next
        {
            Establish context = () =>
                                    {
                                        currentTrack = TestTracks.Get(HarmonicKey.Key9A);
                                        strategy = new OneSemitoneEnergyBoost();
                                        unplayedTracks = new[]
                                                             {
                                                                 TestTracks.Get(HarmonicKey.Key8B), 
                                                                 TestTracks.Get(HarmonicKey.Key4A),
                                                                 TestTracks.Get(HarmonicKey.Key4B),
                                                                 TestTracks.Get(HarmonicKey.Key4A),
                                                                 TestTracks.Get(HarmonicKey.Key8A),
                                                             };
                                    };

            static Track currentTrack;
            static IMixingStrategy strategy;

            Because of = () => suggestedTracks = unplayedTracks.Where(t => strategy.IsCompatible(currentTrack, t));
            static IEnumerable<Track> unplayedTracks;
            static IEnumerable<Track> suggestedTracks;

            It should_suggest_tracks_that_are_one_semitone_up_from_the_current =
                () => suggestedTracks.Select(t => t.Key).ShouldContainOnly(HarmonicKey.Key4A, HarmonicKey.Key4A);
        }
    }
}