using System.Windows.Data;
using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(OpenKeyNotationHarmonicKeyConverter))]
    public class OpenKeyNotationHarmonicKeyConverterSpecs
    {
         public class When_mapping_from_keycode_to_okn
         {
             Establish context = () => converter = new OpenKeyNotationHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Key6B, null, null, null).ShouldEqual("11d");

             static IValueConverter converter;
         }

         public class When_mapping_an_unknown_key_to_okn
         {
             Establish context = () => converter = new OpenKeyNotationHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Unknown, null, null, null).ShouldEqual("Unknown Key");

             static IValueConverter converter;
         }

         public class When_mapping_from_okn_to_keycode
         {
             Establish context = () => converter = new OpenKeyNotationHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("11D", null, null, null).ShouldEqual(HarmonicKey.Key6B);

             static IValueConverter converter;
         }

         public class When_mapping_an_invalid_okn_key_to_keycode
         {
             Establish context = () => converter = new OpenKeyNotationHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("sadasd", null, null, null).ShouldEqual(HarmonicKey.Unknown);

             static IValueConverter converter;
         }
    }
}