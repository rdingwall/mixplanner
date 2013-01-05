using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(TraditionalSymbolsHarmonicKeyConverter))]
    public class TraditionalSymbolsHarmonicKeyConverterSpecs
    {
         public class When_mapping_from_camelot_to_traditional
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Key6B, null, null, null).ShouldEqual("B♭ Major");

             static TraditionalSymbolsHarmonicKeyConverter converter;
         }

         public class When_mapping_an_unknown_camelot_key_to_traditional
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Unknown, null, null, null).ShouldEqual("Unknown Key");

             static TraditionalSymbolsHarmonicKeyConverter converter;
         }

         public class When_mapping_from_traditional_to_camelot
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("B♭ Major", null, null, null).ShouldEqual(HarmonicKey.Key6B);

             static TraditionalSymbolsHarmonicKeyConverter converter;
         }

         public class When_mapping_an_unknown_traditional_key_to_camelot
         {
             Establish context = () => converter = new TraditionalSymbolsHarmonicKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("sadasd", null, null, null).ShouldEqual(HarmonicKey.Unknown);

             static TraditionalSymbolsHarmonicKeyConverter converter;
         }
    }
}