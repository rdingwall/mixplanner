using Machine.Specifications;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(PlaybackSpeed))]
    public class PlaybackSpeedSpecs
    {
        public class When_increasing_the_playback_speed_by_three
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(3);

            It should_increase_the_speed = () => speed.PercentIncrease.ShouldEqual(3);

            It should_increase_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(131.84, 0.01);

            It should_increase_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key2A);
        }

        public class When_increasing_the_playback_speed_by_zero
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(0);

            It should_use_the_original_speed = () => speed.PercentIncrease.ShouldEqual(0);

            It should_use_the_original_bpm = () => speed.ActualBpm.ShouldBeCloseTo(128, 0.01);

            It should_use_the_original_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key7A);
        }

        public class When_increasing_the_playback_speed_by_six
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(6);

            It should_increase_the_speed = () => speed.PercentIncrease.ShouldEqual(6);

            It should_increase_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(135.68, 0.01);

            It should_increase_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key9A);
        }

        public class When_decreasing_the_playback_speed_by_three
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(-3);

            It should_decrease_the_speed = () => speed.PercentIncrease.ShouldEqual(-3);

            It should_decrease_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(124.16, 0.01);

            It should_decrease_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key12A);
        }

        public class When_decreasing_the_playback_speed_by_six
        {
            Establish context = () => speed = new PlaybackSpeed(HarmonicKey.Key7A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(-6);

            It should_decrease_the_speed = () => speed.PercentIncrease.ShouldEqual(-6);

            It should_decrease_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(120.32, 0.01);

            It should_decrease_the_key = () => speed.ActualKey.ShouldEqual(HarmonicKey.Key5A);
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_under_plus_3
        {
            It should_be_within_range =
                () => TestTracks.PlaybackSpeed(128).IsWithinBpmRange(TestTracks.PlaybackSpeed(131))
                                .ShouldBeTrue();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_over_plus_3
        {
            It should_not_be_within_range =
                () => TestTracks.PlaybackSpeed(128).IsWithinBpmRange(TestTracks.PlaybackSpeed(133))
                                .ShouldBeFalse();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_over_minus_3
        {
            It should_be_within_range =
                () => TestTracks.PlaybackSpeed(128).IsWithinBpmRange(TestTracks.PlaybackSpeed(125))
                                .ShouldBeTrue();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_under_minus_3
        {
            It should_not_be_within_range =
                () => TestTracks.PlaybackSpeed(128).IsWithinBpmRange(TestTracks.PlaybackSpeed(120))
                                .ShouldBeFalse();
        }
    }
}