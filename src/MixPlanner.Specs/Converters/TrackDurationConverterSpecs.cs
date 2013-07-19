using System;
using Machine.Specifications;
using MixPlanner.Converters;

namespace MixPlanner.Specs.Converters
{
    [Subject(typeof(TrackDurationConverter))]
    public class TrackDurationConverterSpecs
    {
        public class When_converting_from_a_timespan_to_track_duration
        {
            Because of = () => str = new TrackDurationConverter().Convert(
                new TimeSpan(minutes: 17, seconds: 42, milliseconds: 14, hours: 0, days: 0), typeof(string), null, null);

            It should_format_correctly = () => str.ShouldEqual("17:42");

            static object str;
        }

        public class When_converting_from_a_timespan_to_track_duration_with_single_digit_minutes
        {
            Because of = () => str = new TrackDurationConverter().Convert(
                new TimeSpan(minutes: 7, seconds: 42, milliseconds: 14, hours: 0, days: 0), typeof(string), null, null);

            It should_format_correctly = () => str.ShouldEqual("7:42");

            static object str;
        }
         
    }
}