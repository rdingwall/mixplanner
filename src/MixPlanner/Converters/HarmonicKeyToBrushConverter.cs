using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    public class HarmonicKeyToBrushConverter : IValueConverter
    {
        // Based on middle of square color from this
        // http://www.harmonic-mixing.com/Images/camelotHarmonicMixing.jpg
        static readonly IDictionary<HarmonicKey, Color> ColorWheel =
            new Dictionary<HarmonicKey, Color>
                {
                    {HarmonicKey.Key1A, Color.FromRgb(182, 255, 226)},
                    {HarmonicKey.Key2A, Color.FromRgb(196, 255, 189)},
                    {HarmonicKey.Key3A, Color.FromRgb(212, 249, 169)},
                    {HarmonicKey.Key4A, Color.FromRgb(227, 226, 169)},
                    {HarmonicKey.Key5A, Color.FromRgb(247, 195, 171)},
                    {HarmonicKey.Key6A, Color.FromRgb(255, 175, 184)},
                    {HarmonicKey.Key7A, Color.FromRgb(247, 174, 204)},
                    {HarmonicKey.Key8A, Color.FromRgb(225, 173, 235)},
                    {HarmonicKey.Key9A, Color.FromRgb(209, 174, 255)},
                    {HarmonicKey.Key10A, Color.FromRgb(197, 192, 255)},
                    {HarmonicKey.Key11A, Color.FromRgb(185, 231, 255)},
                    {HarmonicKey.Key12A, Color.FromRgb(174, 254, 253)},
                    {HarmonicKey.Key1B, Color.FromRgb(143, 255, 210)},
                    {HarmonicKey.Key2B, Color.FromRgb(161, 255, 156)},
                    {HarmonicKey.Key3B, Color.FromRgb(182, 244, 117)},
                    {HarmonicKey.Key4B, Color.FromRgb(208, 210, 113)},
                    {HarmonicKey.Key5B, Color.FromRgb(248, 158, 124)},
                    {HarmonicKey.Key6B, Color.FromRgb(255, 127, 144)},
                    {HarmonicKey.Key7B, Color.FromRgb(237, 126, 179)},
                    {HarmonicKey.Key8B, Color.FromRgb(210, 127, 217)},
                    {HarmonicKey.Key9B, Color.FromRgb(181, 126, 254)},
                    {HarmonicKey.Key10B, Color.FromRgb(164, 158, 255)},
                    {HarmonicKey.Key11B, Color.FromRgb(148, 213, 255)},
                    {HarmonicKey.Key12B, Color.FromRgb(130, 253, 255)}
                   
                };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (HarmonicKey) value;

            Color color;
            if (!ColorWheel.TryGetValue(key, out color))
                 color = Colors.LightGray;

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}