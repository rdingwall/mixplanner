using System;

namespace MixPlanner.DomainModel.AutoMixing
{
    public static class BpmRangeCalculator
    {
        // Doesn't really matter what we use for the base but this is by far
        // the most common.
        const double BaseBpm = 128;

        static readonly BpmRange InvalidRange = new BpmRange(Int32.MinValue, Double.NaN, Double.NaN);

        public static BpmRange GetNumberOfDegreesDistanceFromMean(double bpm)
        {
            if (Double.IsNaN(bpm))
                return InvalidRange;

            double difference = (bpm/BaseBpm)-1;
            double degreesDifference = difference/HarmonicKeyChangeInterval.Value;
            var iDegreesDifference = (int)degreesDifference;

            return new BpmRange(iDegreesDifference,
                                GetLowerBpmBound(iDegreesDifference),
                                GetUpperBpmBound(iDegreesDifference));
        }

        public static double GetLowerBpmBound(int degrees)
        {
            return 128 + ((degrees - 1)*HarmonicKeyChangeInterval.Value);
        }

        public static double GetUpperBpmBound(int degrees)
        {
            return 128 + (degrees * HarmonicKeyChangeInterval.Value);
        }
    }
}