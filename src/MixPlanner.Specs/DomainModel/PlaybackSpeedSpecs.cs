using Machine.Specifications;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(PlaybackSpeed))]
    public class PlaybackSpeedSpecs
    {
        public class When_increasing_the_playback_speed_by_three_percent
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(1.03);

            It should_increase_the_speed = () => speed.Speed.ShouldEqual(1.03);

            It should_increase_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(131.84, 0.01);

            It should_not_alter_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key7A);
        }

        public class When_increasing_the_playback_speed_by_zero_percent
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(1.0);

            It should_use_the_original_speed = () => speed.Speed.ShouldEqual(1.0);

            It should_use_the_original_bpm = () => speed.ActualBpm.ShouldBeCloseTo(128, 0.01);

            It should_use_the_original_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key7A);
        }

        public class When_increasing_the_playback_speed_by_six_percent
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(1.06);

            It should_increase_the_speed = () => speed.Speed.ShouldEqual(1.06);

            It should_increase_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(135.68, 0.01);

            It should_increase_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key2A);
        }

        public class When_decreasing_the_playback_speed_by_three_percent
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(.97);

            It should_decrease_the_speed = () => speed.Speed.ShouldEqual(.97);

            It should_decrease_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(124.16, 0.01);

            It should_not_alter_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key7A);
        }

        public class When_decreasing_the_playback_speed_by_six_percent
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(.94);

            It should_decrease_the_speed = () => speed.Speed.ShouldEqual(.94);

            It should_decrease_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(120.32, 0.01);

            It should_decrease_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key12A);
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_under_plus_three_percent
        {
            It should_be_within_range =
                () => TestPlaybackSpeeds.PlaybackSpeed(128).IsWithinBpmRange(TestPlaybackSpeeds.PlaybackSpeed(131))
                                .ShouldBeTrue();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_over_plus_six_percent
        {
            It should_not_be_within_range =
                () => TestPlaybackSpeeds.PlaybackSpeed(128).IsWithinBpmRange(TestPlaybackSpeeds.PlaybackSpeed(136))
                                .ShouldBeFalse();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_over_minus_3
        {
            It should_be_within_range =
                () => TestPlaybackSpeeds.PlaybackSpeed(128).IsWithinBpmRange(TestPlaybackSpeeds.PlaybackSpeed(125))
                                .ShouldBeTrue();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_under_minus_3
        {
            It should_not_be_within_range =
                () => TestPlaybackSpeeds.PlaybackSpeed(128).IsWithinBpmRange(TestPlaybackSpeeds.PlaybackSpeed(120))
                                .ShouldBeFalse();
        }

        public class When_increasing_the_playback_speed_with_an_unknown_key
        {
            Establish context = () => speed = TestPlaybackSpeeds.PlaybackSpeed(HarmonicKey.Unknown);

            Because of = () => speed.SetSpeed(1.5);

            It should_still_have_an_unknown_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Unknown);

            static PlaybackSpeed speed; 
        }
    }
}