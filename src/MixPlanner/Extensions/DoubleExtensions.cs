// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class DoubleExtensions
    {
        public static double RoundToNearest(this double value, double interval)
        {
            if (interval <= 0)
                throw new ArgumentOutOfRangeException("interval", interval, "Interval must be greater than zero.");

            // From http://stackoverflow.com/a/1531708/91551
            return Math.Round(value / interval) * interval;
        }
    }
}