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
        static readonly IDictionary<Key, Color> ColorWheel =
            new Dictionary<Key, Color>
                {
                    {Key.Key1A, Color.FromRgb(182, 255, 226)},
                    {Key.Key2A, Color.FromRgb(196, 255, 189)},
                    {Key.Key3A, Color.FromRgb(212, 249, 169)},
                    {Key.Key4A, Color.FromRgb(227, 226, 169)},
                    {Key.Key5A, Color.FromRgb(247, 195, 171)},
                    {Key.Key6A, Color.FromRgb(255, 175, 184)},
                    {Key.Key7A, Color.FromRgb(247, 174, 204)},
                    {Key.Key8A, Color.FromRgb(225, 173, 235)},
                    {Key.Key9A, Color.FromRgb(209, 174, 255)},
                    {Key.Key10A, Color.FromRgb(197, 192, 255)},
                    {Key.Key11A, Color.FromRgb(185, 231, 255)},
                    {Key.Key12A, Color.FromRgb(174, 254, 253)},
                    {Key.Key1B, Color.FromRgb(143, 255, 210)},
                    {Key.Key2B, Color.FromRgb(161, 255, 156)},
                    {Key.Key3B, Color.FromRgb(182, 244, 117)},
                    {Key.Key4B, Color.FromRgb(208, 210, 113)},
                    {Key.Key5B, Color.FromRgb(248, 158, 124)},
                    {Key.Key6B, Color.FromRgb(255, 127, 144)},
                    {Key.Key7B, Color.FromRgb(237, 126, 179)},
                    {Key.Key8B, Color.FromRgb(210, 127, 217)},
                    {Key.Key9B, Color.FromRgb(181, 126, 254)},
                    {Key.Key10B, Color.FromRgb(164, 158, 255)},
                    {Key.Key11B, Color.FromRgb(148, 213, 255)},
                    {Key.Key12B, Color.FromRgb(130, 253, 255)}
                   
                };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (Key) value;

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