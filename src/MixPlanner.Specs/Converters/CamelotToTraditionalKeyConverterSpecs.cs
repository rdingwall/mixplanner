using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(CamelotToTraditionalKeyConverter))]
    public class CamelotToTraditionalKeyConverterSpecs
    {
         public class When_mapping_from_camelot_to_traditional
         {
             Establish context = () => converter = new CamelotToTraditionalKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Key6B, null, null, null).ShouldEqual("B♭ Major");

             static CamelotToTraditionalKeyConverter converter;
         }

         public class When_mapping_an_unknown_camelot_key_to_traditional
         {
             Establish context = () => converter = new CamelotToTraditionalKeyConverter();

             It should_return_the_correct_key =
                 () => converter.Convert(HarmonicKey.Unknown, null, null, null).ShouldEqual("Unknown Key");

             static CamelotToTraditionalKeyConverter converter;
         }

         public class When_mapping_from_traditional_to_camelot
         {
             Establish context = () => converter = new CamelotToTraditionalKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("B♭ Major", null, null, null).ShouldEqual(HarmonicKey.Key6B);

             static CamelotToTraditionalKeyConverter converter;
         }

         public class When_mapping_an_unknown_traditional_key_to_camelot
         {
             Establish context = () => converter = new CamelotToTraditionalKeyConverter();

             It should_return_the_correct_key =
                 () => converter.ConvertBack("sadasd", null, null, null).ShouldEqual(HarmonicKey.Unknown);

             static CamelotToTraditionalKeyConverter converter;
         }
    }
}