﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class TraditionalTextHarmonicKeyConverter : IValueConverter
    {
        static readonly IDictionary<HarmonicKey, string> TradtionalKeys;
        static readonly IDictionary<string, HarmonicKey> KeyCodes;

        static TraditionalTextHarmonicKeyConverter()
        {
            TradtionalKeys =
                new Dictionary<HarmonicKey, string>
                    {
                        {HarmonicKey.Key1A, "A-Flat Minor"},
                        {HarmonicKey.Key1B, "B Major"},
                        {HarmonicKey.Key2A, "E-Flat Minor"},
                        {HarmonicKey.Key2B, "F-Sharp Major"},
                        {HarmonicKey.Key3A, "B-Flat Minor"},
                        {HarmonicKey.Key3B, "D-Flat Major"},
                        {HarmonicKey.Key4A, "F Minor"},
                        {HarmonicKey.Key4B, "A-Flat Major"},
                        {HarmonicKey.Key5A, "C Minor"},
                        {HarmonicKey.Key5B, "E-Flat Major"},
                        {HarmonicKey.Key6A, "G Minor"},
                        {HarmonicKey.Key6B, "B-Flat Major"},
                        {HarmonicKey.Key7A, "D Minor"},
                        {HarmonicKey.Key7B, "F Major"},
                        {HarmonicKey.Key8A, "A Minor"},
                        {HarmonicKey.Key8B, "C Major"},
                        {HarmonicKey.Key9A, "E Minor"},
                        {HarmonicKey.Key9B, "G Major"},
                        {HarmonicKey.Key10A, "B Minor"},
                        {HarmonicKey.Key10B, "D Major"},
                        {HarmonicKey.Key11A, "F-Sharp Minor"},
                        {HarmonicKey.Key11B, "A Major"},
                        {HarmonicKey.Key12A, "D-Flat Minor"},
                        {HarmonicKey.Key12B, "E Major"}
                    };

            KeyCodes = TradtionalKeys
                .ToDictionary(k => k.Value, v => v.Key,
                StringComparer.CurrentCultureIgnoreCase);

            // Enharmonic equivalents (only one way so we can read them)
            KeyCodes.Add("G-Sharp Minor", HarmonicKey.Key1A);
            KeyCodes.Add("D-Sharp Minor", HarmonicKey.Key2A);
            KeyCodes.Add("G-Flat Major", HarmonicKey.Key2B);
            KeyCodes.Add("A-Sharp Minor", HarmonicKey.Key3A);
            KeyCodes.Add("C-Sharp Major", HarmonicKey.Key3B);
            KeyCodes.Add("G-Sharp Major", HarmonicKey.Key4B);
            KeyCodes.Add("D-Sharp Major", HarmonicKey.Key5B);
            KeyCodes.Add("A-Sharp Major", HarmonicKey.Key6B);
            KeyCodes.Add("G-Flat Minor", HarmonicKey.Key11A);
            KeyCodes.Add("C-Sharp Minor", HarmonicKey.Key12A);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as HarmonicKey?;

            if (key == null)
                return null;

            string traditionalKey;
            if (TradtionalKeys.TryGetValue(key.Value, out traditionalKey))
                return traditionalKey;

            return "Unknown Key";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var traditionalKey = value as string;

            if (traditionalKey == null)
                return null;

            HarmonicKey key;
            if (KeyCodes.TryGetValue(traditionalKey.Trim(), out key))
                return key;

            return HarmonicKey.Unknown;
        }
    }
}