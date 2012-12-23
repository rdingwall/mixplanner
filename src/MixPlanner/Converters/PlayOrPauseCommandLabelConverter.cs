using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Converters
{
    public class PlayOrPauseCommandLabelConverter : IValueConverter
    {
        readonly IAudioPlayer player;

        public PlayOrPauseCommandLabelConverter() : 
            this(ServiceLocator.Current.GetInstance<IAudioPlayer>())
        {
        }

        public PlayOrPauseCommandLabelConverter(IAudioPlayer player)
        {
            if (player == null) throw new ArgumentNullException("player");
            this.player = player;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var track = (Track) value;
            return player.IsPlaying(track) ? "Pause" : "Play";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}