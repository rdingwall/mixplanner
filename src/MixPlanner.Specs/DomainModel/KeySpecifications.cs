using System;
using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(Key))]
    class KeySpecifications
    {
        public class when_constructing_with_an_invalid_scale
        {
            Because of = () => exception = Catch.Exception(() => new Key("8f"));

            It should_throw_an_argument_out_of_range_exception =
                () => exception.ShouldBe(typeof(InvalidScaleException));

            static Exception exception;
        }

        public class when_constructing_with_an_invalid_pitch
        {
            Because of = () => exception = Catch.Exception(() => new Key("36A"));

            It should_throw_an_argument_out_of_range_exception =
                () => exception.ShouldBe(typeof(InvalidPitchException));

            static Exception exception;
        }

        public class when_constructing_a_valid_key
        {
            Because of = () => key = new Key("8A");

            It should_be_the_correct_scale =
                () => key.Scale.ShouldEqual(Scale.Minor);

            It should_be_minor =
                () => key.IsMinor().ShouldBeTrue();

            It should_not_be_major =
                () => key.IsMajor().ShouldBeFalse();

            It should_be_in_the_correct_pitch =
                () => key.Pitch.ShouldEqual(8);

            static Key key;
        }

        public class when_constructing_from_split_scale_and_pitch
        {
            Because of = () => key = new Key(8, Scale.Minor);

            It should_be_the_correct_scale =
                () => key.Scale.ShouldEqual(Scale.Minor);

            It should_be_minor =
                () => key.IsMinor().ShouldBeTrue();

            It should_not_be_major =
                () => key.IsMajor().ShouldBeFalse();

            It should_be_in_the_correct_pitch =
                () => key.Pitch.ShouldEqual(8);

            static Key key;
        }

        public class when_comparing_two_instances_of_the_same_key
        {
            It should_consider_them_equal =
                () => new Key("12A").ShouldEqual(new Key("12A"));
        }

        public class when_comparing_two_different_keys
        {
            It should_not_consider_them_equal =
                () => new Key("8B").ShouldNotEqual(new Key("12A"));
        }

        public class when_rendering_as_a_string
        {
            Because of = () => str = new Key("8a").ToString();

            It should_display_with_the_pitch_class_and_scale =
                () => str.ShouldEqual("8A");

            static string str;
        }

        public class when_using_key_constants
        {
            It should_provide_a_constant_for_1A = () => Key.Key1A.ToString().ShouldEqual("1A");
            It should_provide_a_constant_for_2A = () => Key.Key2A.ToString().ShouldEqual("2A");
            It should_provide_a_constant_for_3A = () => Key.Key3A.ToString().ShouldEqual("3A");
            It should_provide_a_constant_for_4A = () => Key.Key4A.ToString().ShouldEqual("4A");
            It should_provide_a_constant_for_5A = () => Key.Key5A.ToString().ShouldEqual("5A");
            It should_provide_a_constant_for_6A = () => Key.Key6A.ToString().ShouldEqual("6A");
            It should_provide_a_constant_for_7A = () => Key.Key7A.ToString().ShouldEqual("7A");
            It should_provide_a_constant_for_8A = () => Key.Key8A.ToString().ShouldEqual("8A");
            It should_provide_a_constant_for_9A = () => Key.Key9A.ToString().ShouldEqual("9A");
            It should_provide_a_constant_for_10A = () => Key.Key10A.ToString().ShouldEqual("10A");
            It should_provide_a_constant_for_11A = () => Key.Key11A.ToString().ShouldEqual("11A");
            It should_provide_a_constant_for_12A = () => Key.Key12A.ToString().ShouldEqual("12A");

            It should_provide_a_constant_for_1B = () => Key.Key1B.ToString().ShouldEqual("1B");
            It should_provide_a_constant_for_2B = () => Key.Key2B.ToString().ShouldEqual("2B");
            It should_provide_a_constant_for_3B = () => Key.Key3B.ToString().ShouldEqual("3B");
            It should_provide_a_constant_for_4B = () => Key.Key4B.ToString().ShouldEqual("4B");
            It should_provide_a_constant_for_5B = () => Key.Key5B.ToString().ShouldEqual("5B");
            It should_provide_a_constant_for_6B = () => Key.Key6B.ToString().ShouldEqual("6B");
            It should_provide_a_constant_for_7B = () => Key.Key7B.ToString().ShouldEqual("7B");
            It should_provide_a_constant_for_8B = () => Key.Key8B.ToString().ShouldEqual("8B");
            It should_provide_a_constant_for_9B = () => Key.Key9B.ToString().ShouldEqual("9B");
            It should_provide_a_constant_for_10B = () => Key.Key10B.ToString().ShouldEqual("10B");
            It should_provide_a_constant_for_11B = () => Key.Key11B.ToString().ShouldEqual("11B");
            It should_provide_a_constant_for_12B = () => Key.Key12B.ToString().ShouldEqual("12B");
        }

        public class when_increasing_the_pitch
        {
            It should_add_the_pitch =
                () => Key.Key5A.IncreasePitch(7).ShouldEqual(Key.Key12A);
        }

        public class when_increasing_the_pitch_past_twelve
        {
            It should_go_around_again =
                () => Key.Key12A.IncreasePitch(7).ShouldEqual(Key.Key7A);
        }

        public class when_switching_to_minor
        {
            It should_change_the_scale =
                () => new Key("12B").ToMinor().ShouldEqual(new Key("12A"));
        }

        public class when_switching_to_major
        {
            It should_change_the_scale =
                () => new Key("12A").ToMajor().ShouldEqual(new Key("12B"));
        }

        public class when_switching_to_major_but_was_already_major
        {
            It should_keep_the_same_scale =
                () => new Key("12B").ToMajor().ShouldEqual(new Key("12B"));
        }

        public class when_switching_to_minor_but_was_already_minor
        {
            It should_keep_the_same_scale =
                () => new Key("12A").ToMinor().ShouldEqual(new Key("12A"));
        }

        public class when_try_parsing_a_valid_key
        {
            Because of = () => result = Key.TryParse("12A", out key);
            static Key key;
            static bool result;

            It should_return_true = () => result.ShouldBeTrue();

            It should_return_the_key = () => key.ShouldEqual(Key.Key12A);
        }

        public class when_try_parsing_an_invalid_key
        {
            Because of = () => result = Key.TryParse("xyz", out key);
            static Key key;
            static bool result;

            It should_return_true = () => result.ShouldBeFalse();

            It should_return_null = () => key.ShouldBeNull();
        }
    }
}
