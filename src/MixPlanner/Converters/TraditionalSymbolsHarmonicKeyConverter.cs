﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using MixPlanner.DomainModel;
using System.Linq;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class TraditionalSymbolsHarmonicKeyConverter : IValueConverter
    {
        static readonly IDictionary<HarmonicKey, string> TradtionalKeys;
        static readonly IDictionary<string, HarmonicKey> KeyCodes;

        static TraditionalSymbolsHarmonicKeyConverter()
        {
            TradtionalKeys = 
            new Dictionary<HarmonicKey, string>
                {
                    {HarmonicKey.Key1A, "A♭ Minor"},
                    {HarmonicKey.Key1B, "B Major"},
                    {HarmonicKey.Key2A, "E♭ Minor"},
                    {HarmonicKey.Key2B, "F♯ Major"},
                    {HarmonicKey.Key3A, "B♭ Minor"},
                    {HarmonicKey.Key3B, "D♭ Major"},
                    {HarmonicKey.Key4A, "F Minor"},
                    {HarmonicKey.Key4B, "A♭ Major"},
                    {HarmonicKey.Key5A, "C Minor"},
                    {HarmonicKey.Key5B, "E♭ Major"},
                    {HarmonicKey.Key6A, "G Minor"},
                    {HarmonicKey.Key6B, "B♭ Major"},
                    {HarmonicKey.Key7A, "D Minor"},
                    {HarmonicKey.Key7B, "F Major"},
                    {HarmonicKey.Key8A, "A Minor"},
                    {HarmonicKey.Key8B, "C Major"},
                    {HarmonicKey.Key9A, "E Minor"},
                    {HarmonicKey.Key9B, "G Major"},
                    {HarmonicKey.Key10A, "B Minor"},
                    {HarmonicKey.Key10B, "D Major"},
                    {HarmonicKey.Key11A, "F♯ Minor"},
                    {HarmonicKey.Key11B, "A Major"},
                    {HarmonicKey.Key12A, "D♭ Minor"},
                    {HarmonicKey.Key12B, "E Major"}
                };

            KeyCodes = TradtionalKeys
                .ToDictionary(k => k.Value, v => v.Key);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as HarmonicKey;

            if (key == null)
                return null;

            string traditionalKey;
            if (TradtionalKeys.TryGetValue(key, out traditionalKey))
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