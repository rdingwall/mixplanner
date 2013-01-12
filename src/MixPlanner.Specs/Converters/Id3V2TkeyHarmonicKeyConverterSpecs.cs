using System.Windows.Data;
using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(Id3v2TkeyHarmonicKeyConverter))]
    public class Id3V2TkeyHarmonicKeyConverterSpecs
    {
         public class When_mapping_from_keycode_to_id3v2_tkey
         {
             Establish context = () => converter = new Id3v2TkeyHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Key6B, null, null, null).ShouldEqual("Bb");

             static IValueConverter converter;
         }

         public class When_mapping_an_unknown_key_to_id3v2_tkey
         {
             Establish context = () => converter = new Id3v2TkeyHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Unknown, null, null, null).ShouldEqual("Unknown Key");

             static IValueConverter converter;
         }

         public class When_mapping_from_id3v2_tkey_to_keycode
         {
             Establish context = () => converter = new Id3v2TkeyHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("Bb", null, null, null).ShouldEqual(HarmonicKey.Key6B);

             static IValueConverter converter;
         }

         public class When_mapping_an_off_key_to_keycode
         {
             Establish context = () => converter = new Id3v2TkeyHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("o", null, null, null).ShouldEqual(HarmonicKey.Unknown);

             static IValueConverter converter;
         }

         public class When_mapping_an_invalid_id3v2_tkey_key_to_keycode
         {
             Establish context = () => converter = new Id3v2TkeyHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("sadasd", null, null, null).ShouldEqual(HarmonicKey.Unknown);

             static IValueConverter converter;
         }

         public class When_mapping_an_enharmonic_id3v2_tkey_key_to_keycode
         {
             Establish context = () => converter = new Id3v2TkeyHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("C#m", null, null, null).ShouldEqual(HarmonicKey.Key12A);

             static IValueConverter converter;
         }
    }
}