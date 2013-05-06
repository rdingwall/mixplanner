using System.Windows.Data;
using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(BeatportHarmonicKeyConverter))]
    public class BeatportHarmonicKeyConverterSpecs
    {
        public class When_mapping_from_keycode_to_beatport
        {
            Establish context = () => converter = new BeatportHarmonicKeyConverter();

            It should_return_the_correct_key =
                () => converter.Convert(HarmonicKey.Key6B, null, null, null).ShouldEqual("A#maj");

            static IValueConverter converter;
        }

        public class When_mapping_an_unknown_key_to_beatport
        {
            Establish context = () => converter = new BeatportHarmonicKeyConverter();

            It should_return_the_correct_key =
                () => converter.Convert(HarmonicKey.Unknown, null, null, null).ShouldEqual("Unknown Key");

            static IValueConverter converter;
        }

        public class When_mapping_an_off_key_to_keycode
        {
            Establish context = () => converter = new BeatportHarmonicKeyConverter();

            It should_return_the_correct_key =
                () => converter.ConvertBack("o", null, null, null).ShouldEqual(HarmonicKey.Unknown);

            static IValueConverter converter;
        }

         public class When_mapping_from_beatport_minor_to_keycode
         {
             Establish context = () => converter = new BeatportHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("Fmin", null, null, null).ShouldEqual(HarmonicKey.Key4A);

             static IValueConverter converter;
         }

         public class When_mapping_from_beatport_major_to_keycode
         {
             Establish context = () => converter = new BeatportHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("A#min", null, null, null).ShouldEqual(HarmonicKey.Key3A);

             static IValueConverter converter;
         }
    }
}