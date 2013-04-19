using System;
using Machine.Specifications;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(HarmonicKey))]
    class HarmonicKeySpecs
    {
        public class when_constructing_with_an_invalid_scale
        {
            Because of = () => exception = Catch.Exception(() => new HarmonicKey("8f"));

            It should_throw_an_argument_out_of_range_exception =
                () => exception.ShouldBe(typeof(InvalidScaleException));

            static Exception exception;
        }

        public class when_constructing_with_an_invalid_pitch
        {
            Because of = () => exception = Catch.Exception(() => new HarmonicKey("36A"));

            It should_throw_an_argument_out_of_range_exception =
                () => exception.ShouldBe(typeof(InvalidPitchException));

            static Exception exception;
        }

        public class when_constructing_a_valid_key
        {
            Because of = () => key = new HarmonicKey("8A");

            It should_be_the_correct_scale =
                () => key.Scale.ShouldEqual(Scale.Minor);

            It should_be_minor =
                () => key.IsMinor().ShouldBeTrue();

            It should_not_be_major =
                () => key.IsMajor().ShouldBeFalse();

            It should_be_in_the_correct_pitch =
                () => key.Pitch.ShouldEqual(8);

            static HarmonicKey key;
        }

        public class when_constructing_from_split_scale_and_pitch
        {
            Because of = () => key = new HarmonicKey(8, Scale.Minor);

            It should_be_the_correct_scale =
                () => key.Scale.ShouldEqual(Scale.Minor);

            It should_be_minor =
                () => key.IsMinor().ShouldBeTrue();

            It should_not_be_major =
                () => key.IsMajor().ShouldBeFalse();

            It should_be_in_the_correct_pitch =
                () => key.Pitch.ShouldEqual(8);

            static HarmonicKey key;
        }

        public class when_comparing_two_instances_of_the_same_key
        {
            It should_consider_them_equal =
                () => new HarmonicKey("12A").ShouldEqual(new HarmonicKey("12A"));

            It should_sort_them_the_same =
                () => HarmonicKey.Key12A.CompareTo(HarmonicKey.Key12A).ShouldEqual(0);
        }

        public class when_comparing_two_different_keys
        {
            It should_not_consider_them_equal =
                () => new HarmonicKey("8B").ShouldNotEqual(new HarmonicKey("12A"));

            It should_sort_them_in_the_correct_order =
                () => HarmonicKey.Key8B.CompareTo(HarmonicKey.Key12A).ShouldBeLessThan(0);
        }

        public class when_comparing_the_same_key_with_different_scales
        {
            It should_not_consider_them_equal =
                () => new HarmonicKey("8A").ShouldNotEqual(new HarmonicKey("8B"));

            It should_sort_them_in_the_correct_order =
                () => HarmonicKey.Key8B.CompareTo(HarmonicKey.Key8A).ShouldBeGreaterThan(0);
        }

        public class when_rendering_as_a_string
        {
            Because of = () => str = new HarmonicKey("8a").ToString();

            It should_display_with_the_pitch_class_and_scale =
                () => str.ShouldEqual("8A");

            static string str;
        }

        public class when_using_key_constants
        {
            It should_provide_a_constant_for_1A = () => HarmonicKey.Key1A.ToString().ShouldEqual("1A");
            It should_provide_a_constant_for_2A = () => HarmonicKey.Key2A.ToString().ShouldEqual("2A");
            It should_provide_a_constant_for_3A = () => HarmonicKey.Key3A.ToString().ShouldEqual("3A");
            It should_provide_a_constant_for_4A = () => HarmonicKey.Key4A.ToString().ShouldEqual("4A");
            It should_provide_a_constant_for_5A = () => HarmonicKey.Key5A.ToString().ShouldEqual("5A");
            It should_provide_a_constant_for_6A = () => HarmonicKey.Key6A.ToString().ShouldEqual("6A");
            It should_provide_a_constant_for_7A = () => HarmonicKey.Key7A.ToString().ShouldEqual("7A");
            It should_provide_a_constant_for_8A = () => HarmonicKey.Key8A.ToString().ShouldEqual("8A");
            It should_provide_a_constant_for_9A = () => HarmonicKey.Key9A.ToString().ShouldEqual("9A");
            It should_provide_a_constant_for_10A = () => HarmonicKey.Key10A.ToString().ShouldEqual("10A");
            It should_provide_a_constant_for_11A = () => HarmonicKey.Key11A.ToString().ShouldEqual("11A");
            It should_provide_a_constant_for_12A = () => HarmonicKey.Key12A.ToString().ShouldEqual("12A");

            It should_provide_a_constant_for_1B = () => HarmonicKey.Key1B.ToString().ShouldEqual("1B");
            It should_provide_a_constant_for_2B = () => HarmonicKey.Key2B.ToString().ShouldEqual("2B");
            It should_provide_a_constant_for_3B = () => HarmonicKey.Key3B.ToString().ShouldEqual("3B");
            It should_provide_a_constant_for_4B = () => HarmonicKey.Key4B.ToString().ShouldEqual("4B");
            It should_provide_a_constant_for_5B = () => HarmonicKey.Key5B.ToString().ShouldEqual("5B");
            It should_provide_a_constant_for_6B = () => HarmonicKey.Key6B.ToString().ShouldEqual("6B");
            It should_provide_a_constant_for_7B = () => HarmonicKey.Key7B.ToString().ShouldEqual("7B");
            It should_provide_a_constant_for_8B = () => HarmonicKey.Key8B.ToString().ShouldEqual("8B");
            It should_provide_a_constant_for_9B = () => HarmonicKey.Key9B.ToString().ShouldEqual("9B");
            It should_provide_a_constant_for_10B = () => HarmonicKey.Key10B.ToString().ShouldEqual("10B");
            It should_provide_a_constant_for_11B = () => HarmonicKey.Key11B.ToString().ShouldEqual("11B");
            It should_provide_a_constant_for_12B = () => HarmonicKey.Key12B.ToString().ShouldEqual("12B");
        }

        public class when_increasing_the_pitch
        {
            It should_add_the_pitch =
                () => HarmonicKey.Key5A.Advance(7).ShouldEqual(HarmonicKey.Key12A);
        }

        public class when_increasing_the_pitch_by_a_negative_value
        {
            It should_add_the_pitch =
                () => HarmonicKey.Key5A.Advance(-7).ShouldEqual(HarmonicKey.Key10A);
        }

        public class when_increasing_the_pitch_past_twelve
        {
            It should_go_around_again =
                () => HarmonicKey.Key12A.Advance(7).ShouldEqual(HarmonicKey.Key7A);
        }

        public class when_increasing_the_pitch_more_than_twelve
        {
            It should_go_around_again =
                () => HarmonicKey.Key12A.Advance(26).ShouldEqual(HarmonicKey.Key2A);
        }

        public class when_increasing_the_pitch_by_a_negative_value_more_than_twelve
        {
            It should_add_the_pitch =
                () => HarmonicKey.Key5A.Advance(-14).ShouldEqual(HarmonicKey.Key3A);
        }

        public class when_increasing_the_pitch_by_a_negative_value_more_than_twenty_four
        {
            It should_add_the_pitch =
                () => HarmonicKey.Key5A.Advance(-26).ShouldEqual(HarmonicKey.Key3A);
        }

        public class when_switching_to_minor
        {
            It should_change_the_scale =
                () => new HarmonicKey("12B").ToMinor().ShouldEqual(new HarmonicKey("12A"));
        }

        public class when_switching_to_major
        {
            It should_change_the_scale =
                () => new HarmonicKey("12A").ToMajor().ShouldEqual(new HarmonicKey("12B"));
        }

        public class when_switching_to_major_but_was_already_major
        {
            It should_keep_the_same_scale =
                () => new HarmonicKey("12B").ToMajor().ShouldEqual(new HarmonicKey("12B"));
        }

        public class when_switching_to_minor_but_was_already_minor
        {
            It should_keep_the_same_scale =
                () => new HarmonicKey("12A").ToMinor().ShouldEqual(new HarmonicKey("12A"));
        }

        public class when_try_parsing_a_valid_key
        {
            Because of = () => result = HarmonicKey.TryParse("12A", out key);
            static HarmonicKey key;
            static bool result;

            It should_return_true = () => result.ShouldBeTrue();

            It should_return_the_key = () => key.ShouldEqual(HarmonicKey.Key12A);
        }

        public class when_try_parsing_an_invalid_key
        {
            Because of = () => result = HarmonicKey.TryParse("xyz", out key);
            static HarmonicKey key;
            static bool result;

            It should_return_true = () => result.ShouldBeFalse();

            It should_return_unknown = () => key.IsUnknown.ShouldBeTrue();
        }

        public class When_initializing_a_default_struct
        {
            static HarmonicKey key;

            It should_be_unknown = () => key.IsUnknown.ShouldBeTrue();

            It should_be_of_unknown_scale = () => key.Scale.ShouldEqual(Scale.Unknown);

            It should_be_of_zero_pitch = () => key.Pitch.ShouldEqual(0);
        }
    }
}
