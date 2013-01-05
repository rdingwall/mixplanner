using System;
using System.Globalization;
using System.Windows.Data;

namespace MixPlanner.Converters
{
    // From http://www.amazedsaint.com/2009/08/mvvm-binding-multiple-radio-buttons-to.html
    [ValueConversion(typeof(bool), typeof(Enum), ParameterType=typeof(Enum))]
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum foo;
            return parameter.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}