using Machine.Specifications;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(PlaybackSpeed))]
    public class PlaybackSpeedSpecs
    {
        public class When_increasing_the_playback_speed_by_three
        {
            Establish context = () => speed = TestPlaybackSpeeds.Get(HarmonicKey.Key7A, HarmonicKey.Key8A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(3);

            It should_increase_the_speed = () => speed.PercentIncrease.ShouldEqual(3);

            It should_increase_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(131.84, 0.01);

            It should_increase_the_starting_key = () => speed.ActualStartingKey.ShouldEqual(HarmonicKey.Key2A);

            It should_increase_the_ending_key = () => speed.ActualEndingKey.ShouldEqual(HarmonicKey.Key3A);
        }

        public class When_increasing_the_playback_speed_by_zero
        {
            Establish context = () => speed = TestPlaybackSpeeds.Get(HarmonicKey.Key7A, HarmonicKey.Key8A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(0);

            It should_use_the_original_speed = () => speed.PercentIncrease.ShouldEqual(0);

            It should_use_the_original_bpm = () => speed.ActualBpm.ShouldBeCloseTo(128, 0.01);

            It should_use_the_original_starting_key = () => speed.ActualStartingKey.ShouldEqual(HarmonicKey.Key7A);
            
            It should_use_the_original_ending_key = () => speed.ActualEndingKey.ShouldEqual(HarmonicKey.Key8A);
        }

        public class When_increasing_the_playback_speed_by_six
        {
            Establish context = () => speed = TestPlaybackSpeeds.Get(HarmonicKey.Key7A, HarmonicKey.Key8A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(6);

            It should_increase_the_speed = () => speed.PercentIncrease.ShouldEqual(6);

            It should_increase_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(135.68, 0.01);

            It should_increase_the_starting_key = () => speed.ActualStartingKey.ShouldEqual(HarmonicKey.Key9A);
            
            It should_increase_the_ending_key = () => speed.ActualEndingKey.ShouldEqual(HarmonicKey.Key10A);
        }

        public class When_decreasing_the_playback_speed_by_three
        {
            Establish context = () => speed = TestPlaybackSpeeds.Get(HarmonicKey.Key7A, HarmonicKey.Key8A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(-3);

            It should_decrease_the_speed = () => speed.PercentIncrease.ShouldEqual(-3);

            It should_decrease_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(124.16, 0.01);

            It should_decrease_the_starting_key = () => speed.ActualStartingKey.ShouldEqual(HarmonicKey.Key12A);
            
            It should_decrease_the_ending_key = () => speed.ActualEndingKey.ShouldEqual(HarmonicKey.Key1A);
        }

        public class When_decreasing_the_playback_speed_by_six
        {
            Establish context = () => speed = TestPlaybackSpeeds.Get(HarmonicKey.Key7A, HarmonicKey.Key8A, 128);

            static PlaybackSpeed speed;

            Because of = () => speed.SetSpeed(-6);

            It should_decrease_the_speed = () => speed.PercentIncrease.ShouldEqual(-6);

            It should_decrease_the_bpm = () => speed.ActualBpm.ShouldBeCloseTo(120.32, 0.01);

            It should_decrease_the_starting_key = () => speed.ActualStartingKey.ShouldEqual(HarmonicKey.Key5A);

            It should_decrease_the_ending_key = () => speed.ActualEndingKey.ShouldEqual(HarmonicKey.Key6A);
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_under_plus_3
        {
            It should_be_within_range =
                () => TestPlaybackSpeeds.Get(128).IsWithinBpmRange(TestPlaybackSpeeds.Get(131))
                                .ShouldBeTrue();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_over_plus_3
        {
            It should_not_be_within_range =
                () => TestPlaybackSpeeds.Get(128).IsWithinBpmRange(TestPlaybackSpeeds.Get(133))
                                .ShouldBeFalse();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_over_minus_3
        {
            It should_be_within_range =
                () => TestPlaybackSpeeds.Get(128).IsWithinBpmRange(TestPlaybackSpeeds.Get(125))
                                .ShouldBeTrue();
        }

        public class When_comparing_to_see_if_the_playback_speed_is_within_range_under_minus_3
        {
            It should_not_be_within_range =
                () => TestPlaybackSpeeds.Get(128).IsWithinBpmRange(TestPlaybackSpeeds.Get(120))
                                .ShouldBeFalse();
        }
    }
}