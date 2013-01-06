using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class PlaySpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var speed = System.Convert.ToDouble(value);
            var percentageIncrease = (speed - 1) * 100;

            return string.Format("{0:+0.#;-0.#;0}%", percentageIncrease);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}