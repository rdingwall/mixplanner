using System.Collections.Generic;
using System.Linq;
using Julana.CommandLine.DomainModel;
using Machine.Specifications;

namespace Julana.Specs.DomainModel
{
    [Subject(typeof(Set))]
    public class SetSpecifications
    {
        public class when_beginning_a_set
        {
            static IEnumerable<Track> allTracks;
            static Set set;

            Establish context = () =>
                                    {
                                        allTracks = TestHelper.GetDummyTracks();
                                    };

            Because of = () => set = new Set(allTracks);

            It should_put_all_the_tracks_as_unplayed =
                () => set.UnplayedTracks.ShouldContainOnly(allTracks);

            It should_have_no_tracks_in_the_set_list =
                () => set.TrackList.ShouldBeEmpty();
        }

        public class when_playing_a_track
        {
            static IEnumerable<Track> allTracks;
            static Set set;

            Establish context = () =>
            {
                allTracks = TestHelper.GetDummyTracks();
                set = new Set(allTracks);
                trackToPlay = set.UnplayedTracks.First();
            };

            Because of = () => set.Play(trackToPlay);

            It should_remove_that_track_from_the_unplayed_tracks =
                () => set.UnplayedTracks.ShouldNotContain(trackToPlay);

            It should_add_the_track_to_the_track_list =
                () => set.TrackList.ShouldContainOnly(trackToPlay);

            static Track trackToPlay;
        }

        public class when_playing_a_track_that_wasnt_in_the_unplayed_tracks
        {
            static IEnumerable<Track> allTracks;
            static Set set;

            Establish context = () =>
            {
                allTracks = TestHelper.GetDummyTracks();
                set = new Set(allTracks);
                trackToPlay = new Track("dummy xxX", Key.RandomKey());
            };

            Because of = () => set.Play(trackToPlay);

            It should_add_the_track_to_the_track_list =
                () => set.TrackList.ShouldContainOnly(trackToPlay);

            static Track trackToPlay;
        }
    }
}