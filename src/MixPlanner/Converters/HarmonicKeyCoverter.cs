using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class HarmonicKeyCoverter : IValueConverter
    {
        readonly Lazy<IHarmonicKeyConverterFactory> factory;

        public HarmonicKeyCoverter()
        {
            factory = new Lazy<IHarmonicKeyConverterFactory>(
                () => ServiceLocator.Current.GetInstance<IHarmonicKeyConverterFactory>());
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = factory.Value.GetConverter();
            return converter.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}