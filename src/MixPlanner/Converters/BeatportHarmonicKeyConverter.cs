using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    /// <summary>
    /// Kind of weird format I've found in Beatport tracks.
    /// </summary>
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class BeatportHarmonicKeyConverter : IValueConverter
    {
        static readonly IDictionary<HarmonicKey, string> TradtionalKeys;
        static readonly IDictionary<string, HarmonicKey> KeyCodes;

        static BeatportHarmonicKeyConverter()
        {
            TradtionalKeys =
                new Dictionary<HarmonicKey, string>
                    {
                        {HarmonicKey.Key1A, "G#min"},
                        {HarmonicKey.Key1B, "Bmaj"},
                        {HarmonicKey.Key2A, "D#min"},
                        {HarmonicKey.Key2B, "F#maj"},
                        {HarmonicKey.Key3A, "A#min"},
                        {HarmonicKey.Key3B, "C#maj"},
                        {HarmonicKey.Key4A, "Fmin"},
                        {HarmonicKey.Key4B, "G#maj"},
                        {HarmonicKey.Key5A, "Cmin"},
                        {HarmonicKey.Key5B, "D#maj"},
                        {HarmonicKey.Key6A, "Gmin"},
                        {HarmonicKey.Key6B, "A#maj"},
                        {HarmonicKey.Key7A, "Dmin"},
                        {HarmonicKey.Key7B, "Fmaj"},
                        {HarmonicKey.Key8A, "Amin"},
                        {HarmonicKey.Key8B, "Cmaj"},
                        {HarmonicKey.Key9A, "Emin"},
                        {HarmonicKey.Key9B, "Gmaj"},
                        {HarmonicKey.Key10A, "Bmin"},
                        {HarmonicKey.Key10B, "Dmaj"},
                        {HarmonicKey.Key11A, "F#min"},
                        {HarmonicKey.Key11B, "Amaj"},
                        {HarmonicKey.Key12A, "C#min"},
                        {HarmonicKey.Key12B, "Emaj"}
                    };

            KeyCodes = TradtionalKeys
                .ToDictionary(k => k.Value, v => v.Key,
                StringComparer.CurrentCultureIgnoreCase);

            // Enharmonic variants - I haven't seen flats in this form (they
            // always seem to use the corresponding sharp) but let's be robust
            // just in case.
            KeyCodes.Add("Abmin", HarmonicKey.Key1A);
            KeyCodes.Add("Ebmin", HarmonicKey.Key2A);
            KeyCodes.Add("Bbmin", HarmonicKey.Key3A);
            KeyCodes.Add("Dbmaj", HarmonicKey.Key3B);
            KeyCodes.Add("Abmaj", HarmonicKey.Key4B);
            KeyCodes.Add("Ebmaj", HarmonicKey.Key5B);
            KeyCodes.Add("Bbmaj", HarmonicKey.Key6B);
            KeyCodes.Add("Dbmin", HarmonicKey.Key12A);
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