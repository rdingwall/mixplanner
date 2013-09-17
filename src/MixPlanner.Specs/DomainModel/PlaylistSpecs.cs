using GalaSoft.MvvmLight.Messaging;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using System.Linq;
using MixPlanner.Player;
using Rhino.Mocks;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(Playlist))]
    public class PlaylistSpecs
    {
        public class When_playing_a_track_in_the_mix : FixtureBase
        {
            Because of = () => Player.Stub(p => p.CurrentTrack).Return(Mix.Tracks.ElementAt(2));

            It should_return_the_correct_previous_track =
                () => Playlist.PreviousTrack.ShouldEqual(Mix.Tracks.ElementAt(1));

            It should_return_the_correct_next_track =
                () => Playlist.NextTrack.ShouldEqual(Mix.Tracks.ElementAt(3));
        }

        public class When_playing_a_track_not_in_the_mix : FixtureBase
        {
            Because of = () => Player.Stub(p => p.CurrentTrack).Return(TestTracks.CreateRandomTrack());

            It should_return_the_correct_previous_track =
                () => Playlist.PreviousTrack.ShouldBeNull();

            It should_return_the_correct_next_track =
                () => Playlist.NextTrack.ShouldBeNull();
        }

        public class When_there_are_no_tracks_in_the_mix : FixtureBase
        {
            Because of = () =>
                             {
                                 Player.Stub(p => p.CurrentTrack).Return(Mix.Tracks.ElementAt(2));
                                 Mix.Clear();
                             };

            It should_return_the_correct_previous_track =
                () => Playlist.PreviousTrack.ShouldBeNull();

            It should_return_the_correct_next_track =
                () => Playlist.NextTrack.ShouldBeNull();
        }

        public class When_at_the_end_of_the_mix : FixtureBase
        {
            Because of = () => Player.Stub(p => p.CurrentTrack).Return(Mix.Tracks.ElementAt(4));

            It should_return_the_correct_previous_track =
                () => Playlist.PreviousTrack.ShouldEqual(Mix.Tracks.ElementAt(3));

            It should_return_the_correct_next_track =
                () => Playlist.NextTrack.ShouldBeNull();
        }

        public class When_at_the_start_of_the_mix : FixtureBase
        {
            Because of = () => Player.Stub(p => p.CurrentTrack).Return(Mix.Tracks.ElementAt(0));

            It should_return_the_correct_previous_track =
                () => Playlist.PreviousTrack.ShouldBeNull();

            It should_return_the_correct_next_track =
                () => Playlist.NextTrack.ShouldEqual(Mix.Tracks.ElementAt(1));
        }

        public class When_removing_tracks_from_the_mix : FixtureBase
        {
            Because of = () =>
                             {
                                 Player.Stub(p => p.CurrentTrack).Return(Mix.Tracks.ElementAt(1));
                                 Mix.Remove(Mix[2]);
                                 Mix.Remove(Mix[3]);
                             };

            It should_return_the_correct_previous_track =
                () => Playlist.PreviousTrack.ShouldEqual(Mix.Tracks.ElementAt(0));

            It should_return_the_correct_next_track =
                () => Playlist.NextTrack.ShouldEqual(Mix.Tracks.ElementAt(2));
        }

        public abstract class FixtureBase
        {
            protected static IPlaylist Playlist;
            protected static IMix Mix;
            protected static IMessenger Messenger;
            protected static IAudioPlayer Player;

            Establish context = () =>
            {
                Player = MockRepository.GenerateStub<IAudioPlayer>();
                Mix = TestMixes.CreateRandomMix();
                Messenger = new Messenger();
                Playlist = new Playlist(Player, new TestMixProvider(Mix), Messenger);
            };
        }
    }
}