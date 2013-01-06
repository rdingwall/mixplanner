using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(double?), typeof(string))]
    public class PlaySpeedIncreaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This also returns null for zeros (0 as nullable == null)
            var nullable = value as double?;
            if (!nullable.HasValue)
                return null;

            var increase = System.Convert.ToDouble(value);
            var percentageIncrease = increase * 100;

            return string.Format("{0:+0.#;-0.#;0}%", percentageIncrease);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}