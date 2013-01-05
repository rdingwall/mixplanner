using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Converters
{
    public class PlayPauseCommandLabelConverter : IValueConverter
    {
        // Using a lazy here, not constructor injection because it screws up
        // blend (can't display the XAML preview).
        readonly Lazy<IAudioPlayer> player = 
            new Lazy<IAudioPlayer>(() => ServiceLocator.Current.GetInstance<IAudioPlayer>());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var track = (Track) value;
            return player.Value.IsPlaying(track) ? "Pause" : "Play";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}