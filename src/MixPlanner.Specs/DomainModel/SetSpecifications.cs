using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;
using Rhino.Mocks;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(Set))]
    public class SetSpecifications
    {
        public class scenario
        {
            protected static IEnumerable<Track> allTracks;
            protected static Set set;
        }

        public class when_beginning_a_set : scenario
        {
            Establish context = () =>
                                    {
                                        allTracks = TestHelper.GetDummyTracks();
                                    };

            Because of = () => set = new Set(allTracks, MockRepository.GenerateStub<INextTrackAdvisor>());

            It should_put_all_the_tracks_as_unplayed =
                () => set.UnplayedTracks.ShouldContainOnly(allTracks);

            It should_have_no_tracks_in_the_set_list =
                () => set.TrackList.ShouldBeEmpty();
        }

        public class when_playing_a_track : scenario
        {
            Establish context = () =>
            {
                allTracks = TestHelper.GetDummyTracks();
                set = new Set(allTracks, MockRepository.GenerateStub<INextTrackAdvisor>());
                trackToPlay = set.UnplayedTracks.First();
            };

            Because of = () => set.Play(trackToPlay);

            It should_remove_that_track_from_the_unplayed_tracks =
                () => set.UnplayedTracks.ShouldNotContain(trackToPlay);

            It should_add_the_track_to_the_track_list =
                () => set.TrackList.ShouldContainOnly(trackToPlay);

            static Track trackToPlay;
        }

        public class when_playing_a_track_that_wasnt_in_the_unplayed_tracks : scenario
        {
            Establish context = () =>
            {
                allTracks = TestHelper.GetDummyTracks();
                set = new Set(allTracks, MockRepository.GenerateStub<INextTrackAdvisor>());
                trackToPlay = new Track("dummy xxX", Key.RandomKey());
            };

            Because of = () => set.Play(trackToPlay);

            It should_add_the_track_to_the_track_list =
                () => set.TrackList.ShouldContainOnly(trackToPlay);

            static Track trackToPlay;
        }

        public class when_suggesting_next_tracks : scenario
        {
            static Dictionary<Track, IMixingStrategy> tracks;

            Establish context = () =>
                                    {
                                        advisor = MockRepository.GenerateStub<INextTrackAdvisor>();
                                        tracks = TestHelper.GetDummyTracks()
                                            .ToDictionary(k => k, v => MockRepository.GenerateStub<IMixingStrategy>());
                                        set = new Set(tracks.Keys, advisor);
                                        set.Play(tracks.Keys.First());

                                        advisor.Stub(a => a.GetSuggestionsForNextTrack(set.CurrentTrack, set.UnplayedTracks)).Return(tracks);
                                    };

            Because of = () => suggestedTracks = set.NextTrackSuggestions();

            It should_add_the_track_to_the_track_list =
                () => suggestedTracks.ShouldEqual(tracks);

            static IDictionary<Track, IMixingStrategy> suggestedTracks;
            static INextTrackAdvisor advisor;
        }

        public class when_suggesting_the_very_first_track : scenario
        {
            Establish context = () =>
            {
                allTracks = TestHelper.GetDummyTracks();
                set = new Set(allTracks, MockRepository.GenerateStub<INextTrackAdvisor>());
            };

            Because of = () => suggestedTracks = set.NextTrackSuggestions();

            It should_simply_suggest_everything =
                () => suggestedTracks.Keys.ShouldEqual(allTracks);

            It should_use_the_opening_track_strategy =
                () => suggestedTracks.Values.OfType<OpeningTrack>().Count().ShouldEqual(allTracks.Count());

            static IDictionary<Track, IMixingStrategy> suggestedTracks;
        }
    }
}