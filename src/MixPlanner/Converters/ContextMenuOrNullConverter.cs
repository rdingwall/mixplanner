using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(bool), typeof(ContextMenu))]
    public class ContextMenuOrNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isEnabled = (bool) value;
            if (!isEnabled)
                return null;

            return parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}