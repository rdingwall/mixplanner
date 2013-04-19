using System;
using System.Globalization;
using System.Windows.Data;
using MixPlanner.DomainModel;

namespace MixPlanner.Converters
{
    [ValueConversion(typeof(Transition), typeof(string))]
    public class TransitionDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var transition = value as Transition;
            if (transition == null)
                return null;

            if (transition.IsIntro)
                return ">>> Intro";
            else if (transition.IsOutro)
                return ">>> Outro";

            return ">>> " + transition.Strategy.Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}