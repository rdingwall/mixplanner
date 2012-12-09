using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    public class IsPlayingFlagToPlayPauseStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? "Pause" : "Play";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}