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
        static readonly IValueConverter TraditionalSymbolsConverter = new TraditionalSymbolsHarmonicKeyConverter();
        static readonly IValueConverter TraditionalTextConverter = new TraditionalTextHarmonicKeyConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (HarmonicKey)value;

            var displayMode = GetDisplayMode();

            switch (displayMode)
            {
                case HarmonicKeyDisplayMode.TraditionalWithText:
                    return TraditionalTextConverter.Convert(value, targetType, parameter, culture);

                case HarmonicKeyDisplayMode.TraditionalWithSymbols:
                    return TraditionalSymbolsConverter.Convert(value, targetType, parameter, culture);

                default:
                case HarmonicKeyDisplayMode.Camelot:
                    return key.ToString();
            }
        }

        static HarmonicKeyDisplayMode GetDisplayMode()
        {
            var storage = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            var configuration = storage.Configuration;

            var displayMode = configuration.HarmonicKeyDisplayMode;
            return displayMode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}