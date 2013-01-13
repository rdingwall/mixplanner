using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(HarmonicKey), typeof(string))]
    public class Id3v2TkeyHarmonicKeyConverter : IValueConverter
    {
        static readonly IDictionary<HarmonicKey, string> Tkeys;
        static readonly IDictionary<string, HarmonicKey> KeyCodes;

        static Id3v2TkeyHarmonicKeyConverter()
        {
            // TKEY (from http://id3.org/id3v2.4.0-frames)
            // The 'Initial key' frame contains the musical key in which the sound
            // starts. It is represented as a string with a maximum length of three
            // characters. The ground keys are represented with "A","B","C","D","E",
            // "F" and "G" and halfkeys represented with "b" and "#". Minor is
            // represented as "m", e.g. "Dbm" $00. Off key is represented with an
            // "o" only.
            Tkeys =
                new Dictionary<HarmonicKey, string>
                    {
                        {HarmonicKey.Key1A, "Abm"},
                        {HarmonicKey.Key1B, "B"},
                        {HarmonicKey.Key2A, "Ebm"},
                        {HarmonicKey.Key2B, "F#"},
                        {HarmonicKey.Key3A, "Bbm"},
                        {HarmonicKey.Key3B, "Db"},
                        {HarmonicKey.Key4A, "Fm"},
                        {HarmonicKey.Key4B, "Ab"},
                        {HarmonicKey.Key5A, "Cm"},
                        {HarmonicKey.Key5B, "Eb"},
                        {HarmonicKey.Key6A, "Gm"},
                        {HarmonicKey.Key6B, "Bb"},
                        {HarmonicKey.Key7A, "Dm"},
                        {HarmonicKey.Key7B, "F"},
                        {HarmonicKey.Key8A, "Am"},
                        {HarmonicKey.Key8B, "C"},
                        {HarmonicKey.Key9A, "Em"},
                        {HarmonicKey.Key9B, "G"},
                        {HarmonicKey.Key10A, "Bm"},
                        {HarmonicKey.Key10B, "D"},
                        {HarmonicKey.Key11A, "F#m"},
                        {HarmonicKey.Key11B, "A"},
                        {HarmonicKey.Key12A, "Dbm"},
                        {HarmonicKey.Key12B, "E"}
                    };

            KeyCodes = Tkeys
                .ToDictionary(k => k.Value, v => v.Key,
                              StringComparer.CurrentCultureIgnoreCase);

            // Enharmonic equivalents (only one way so we can read them)
            KeyCodes.Add("G#m", HarmonicKey.Key1A);
            KeyCodes.Add("D#m", HarmonicKey.Key2A);
            KeyCodes.Add("Gb", HarmonicKey.Key2B);
            KeyCodes.Add("A#m", HarmonicKey.Key3A);
            KeyCodes.Add("C#", HarmonicKey.Key3B);
            KeyCodes.Add("G#", HarmonicKey.Key4B);
            KeyCodes.Add("D#", HarmonicKey.Key5B);
            KeyCodes.Add("A#", HarmonicKey.Key6B);
            KeyCodes.Add("Gbm", HarmonicKey.Key11A);
            KeyCodes.Add("C#m", HarmonicKey.Key12A);
            
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as HarmonicKey;

            if (key == null)
                return null;

            string traditionalKey;
            if (Tkeys.TryGetValue(key, out traditionalKey))
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