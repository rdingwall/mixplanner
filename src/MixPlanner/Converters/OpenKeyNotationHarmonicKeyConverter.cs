using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class OpenKeyNotationHarmonicKeyConverter : IValueConverter
    {
        static readonly IDictionary<HarmonicKey, string> Mappings;
        static readonly IDictionary<string, HarmonicKey> KeyCodes;

        static OpenKeyNotationHarmonicKeyConverter()
        {
            // Reference: http://www.beatunes.com/open-key-notation.html
            Mappings =
                new Dictionary<HarmonicKey, string>
                    {
                        {HarmonicKey.Key1A, "6m"},
                        {HarmonicKey.Key1B, "6d"},
                        {HarmonicKey.Key2A, "7m"},
                        {HarmonicKey.Key2B, "7d"},
                        {HarmonicKey.Key3A, "8m"},
                        {HarmonicKey.Key3B, "8d"},
                        {HarmonicKey.Key4A, "9m"},
                        {HarmonicKey.Key4B, "9d"},
                        {HarmonicKey.Key5A, "10m"},
                        {HarmonicKey.Key5B, "10d"},
                        {HarmonicKey.Key6A, "11m"},
                        {HarmonicKey.Key6B, "11d"},
                        {HarmonicKey.Key7A, "12m"},
                        {HarmonicKey.Key7B, "12d"},
                        {HarmonicKey.Key8A, "1m"},
                        {HarmonicKey.Key8B, "1d"},
                        {HarmonicKey.Key9A, "2m"},
                        {HarmonicKey.Key9B, "2d"},
                        {HarmonicKey.Key10A, "3m"},
                        {HarmonicKey.Key10B, "3d"},
                        {HarmonicKey.Key11A, "4m"},
                        {HarmonicKey.Key11B, "4d"},
                        {HarmonicKey.Key12A, "5m"},
                        {HarmonicKey.Key12B, "5d"}
                    };

            KeyCodes = Mappings
                .ToDictionary(k => k.Value, v => v.Key,
                              StringComparer.CurrentCultureIgnoreCase);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as HarmonicKey;

            if (key == null)
                return null;

            string traditionalKey;
            if (Mappings.TryGetValue(key, out traditionalKey))
                return traditionalKey;

            return "Unknown Key";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var traditionalKey = value as string;

            if (traditionalKey == null)
                return null;

            HarmonicKey key;
            if (KeyCodes.TryGetValue(traditionalKey, out key))
                return key;

            return HarmonicKey.Unknown;
        }
    }
}