using Machine.Specifications;
using MixPlanner.Converters;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(PlaySpeedIncreaseConverter))]
    public class PlaySpeedIncreaseConverterSpecs
    {
         public class When_converting_a_positive_playspeed_increase_amount_to_a_string
         {
             Because of = () => str = new PlaySpeedIncreaseConverter().Convert(0.03, null, null, null);

             It should_convert_it_correctly = () => str.ShouldEqual("+3%");
             
             static object str;
         }

         public class When_converting_a_negative_playspeed_increase_amount_to_a_string
         {
             Because of = () => str = new PlaySpeedIncreaseConverter().Convert(-0.03, null, null, null);

             It should_convert_it_correctly = () => str.ShouldEqual("-3%");

             static object str;
         }

         public class When_converting_a_zero_playspeed_increase_amount_to_a_string
         {
             Because of = () => str = new PlaySpeedIncreaseConverter().Convert(0, null, null, null);

             It should_convert_it_correctly = () => str.ShouldBeNull();

             static object str;
         }

         public class When_converting_a_null_nullable_playspeed_increase_amount_to_a_string
         {
             Because of = () => str = new PlaySpeedIncreaseConverter().Convert(default(double?), null, null, null);

             It should_be_blank = () => str.ShouldBeNull();

             static object str;
         }

         public class When_converting_a_null_playspeed_increase_amount_to_a_string
         {
             Because of = () => str = new PlaySpeedIncreaseConverter().Convert(null, null, null, null);

             It should_be_blank = () => str.ShouldBeNull();

             static object str;
         }
    }
}