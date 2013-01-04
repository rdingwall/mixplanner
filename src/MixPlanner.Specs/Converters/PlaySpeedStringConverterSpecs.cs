using Machine.Specifications;
using MixPlanner.Converters;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(PlaySpeedStringConverter))]
    public class PlaySpeedStringConverterSpecs
    {
         public class When_converting_a_positive_play_speed
         {
             Establish context = () => converter = new PlaySpeedStringConverter();

             Because of = () => result = converter.Convert(1.03, typeof (string), null, null);

             It should_display_it_as_a_percentage = () => result.ShouldEqual("+3%");

             static PlaySpeedStringConverter converter;
             static object result;
         }

         public class When_converting_a_negative_play_speed
         {
             Establish context = () => converter = new PlaySpeedStringConverter();

             Because of = () => result = converter.Convert(0.97, typeof(string), null, null);

             It should_display_it_as_a_percentage = () => result.ShouldEqual("-3%");

             static PlaySpeedStringConverter converter;
             static object result;
         }

         public class When_converting_the_default_play_speed
         {
             Establish context = () => converter = new PlaySpeedStringConverter();

             Because of = () => result = converter.Convert(1, typeof(string), null, null);

             It should_display_it_as_a_percentage = () => result.ShouldEqual("0%");

             static PlaySpeedStringConverter converter;
             static object result;
         }
    }
}