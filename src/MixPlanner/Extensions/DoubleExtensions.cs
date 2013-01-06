// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class DoubleExtensions
    {
        const double Tolerance = 0.0001;

        public static double FloorToNearest(this double value, double interval)
        {
            if (interval <= 0)
                throw new ArgumentOutOfRangeException("interval", interval, "Interval must be greater than zero.");

            if (value >= 0)
                return Math.Floor(value/ interval) * interval;

            return Math.Ceiling(value/ interval) * interval;
        }
    }
}