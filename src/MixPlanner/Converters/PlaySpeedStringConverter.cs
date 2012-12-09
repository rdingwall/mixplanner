using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    public class PlaySpeedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playSpeed = (double) value;

            return string.Format("{0:+0.#;-0.#;0}%", playSpeed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}