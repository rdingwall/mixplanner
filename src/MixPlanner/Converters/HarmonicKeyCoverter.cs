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
            IValueConverter converter;
            try
            {
                converter = factory.Value.GetConverter();
            }
            catch (NullReferenceException)
            {
                // Hacky, but allows us to use design mode in Blend where no
                // service locator is registered.
                return null;
            }
            
            return converter.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}