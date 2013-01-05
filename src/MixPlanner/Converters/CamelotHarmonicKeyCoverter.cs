using System;
using System.Globalization;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class CamelotHarmonicKeyCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (HarmonicKey)value;

            return key.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}