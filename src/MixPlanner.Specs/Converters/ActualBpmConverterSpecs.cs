using Machine.Specifications;
using MixPlanner.Converters;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(ActualBpmConverter))]
    public class ActualBpmConverterSpecs
    {
         public class When_converting_a_bpm_with_decimal_places
         {
             Because of = () => formatted = new ActualBpmConverter().Convert(128.934532, null, null, null);

             It should_format_it_to_one_decimal_place = () => formatted.ShouldEqual("128.9");
             static object formatted; 
         }

         public class When_converting_a_whole_number_bpm
         {
             Because of = () => formatted = new ActualBpmConverter().Convert(128, null, null, null);

             It should_not_include_any_decimal_places = () => formatted.ShouldEqual("128");
             static object formatted;
         }
    }
}