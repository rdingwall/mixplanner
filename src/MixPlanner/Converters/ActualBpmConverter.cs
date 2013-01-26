using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class ActualBpmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var bpm = System.Convert.ToDouble(value);

            if (Double.IsNaN(bpm))
                return "Unknown BPM";

            return string.Format("{0:0.#}", bpm);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}