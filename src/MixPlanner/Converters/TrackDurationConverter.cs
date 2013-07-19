using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TrackDurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timespan = (TimeSpan) value;
            return timespan.ToString(@"m\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}