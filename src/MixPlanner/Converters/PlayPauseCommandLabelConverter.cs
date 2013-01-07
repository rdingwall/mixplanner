using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(Track), typeof(string))]
    public class PlayPauseCommandLabelConverter : IValueConverter
    {
        // Using a lazy here, not constructor injection because it screws up
        // blend (can't display the XAML preview).
        readonly Lazy<IAudioPlayer> player = 
            new Lazy<IAudioPlayer>(() => ServiceLocator.Current.GetInstance<IAudioPlayer>());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;

            if (track == null)
                return "";

            return player.Value.IsPlaying(track) ? "Pause" : "Play";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}