using System;
using GalaSoft.MvvmLight.Messaging;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Specs.Player
{
    [Subject(typeof(AudioPlayer))]
    public class AudioPlayerSpecs
    {
        public class When_attempting_to_play_a_corrupt_mp3
        {
            Establish context =
                () =>
                    {
                        track = TestTracks.GetFilenameOnly("12A - 128 - corrupt.mp3");
                        player = new AudioPlayer(new DispatcherMessenger(new Messenger()));
                    };

            Because of = 
                () => exception = Catch.Exception(() => player.PlayOrResumeAsync(track).Wait());

             It should_not_throw_any_exception = () => exception.ShouldBeNull();

            It should_stay_stopped = () => player.IsPlaying().ShouldBeFalse();

            It should_not_change_the_track = () => player.CurrentTrack.ShouldBeNull();

             static Exception exception;
            static IAudioPlayer player;
            static Track track;
        }
    }
}