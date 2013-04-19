using System;
using System.Globalization;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class KeyCodeHarmonicKeyCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as HarmonicKey?;
            if (key == null)
                return null;

            return key.Value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HarmonicKey key;
            if (HarmonicKey.TryParse(value as string, out key))
                return key;

            return HarmonicKey.Unknown;
        }
    }
}