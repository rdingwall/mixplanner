using System;

namespace MixPlanner.DomainModel.AutoMixing
{
    public struct BpmRange : IEquatable<BpmRange>, IComparable<BpmRange>, IComparable
    {
        readonly int degrees;
        readonly double lowerBpmBound;
        readonly double upperBpmBound;

        public int Degrees
        {
            get { return degrees; }
        }

        public double LowerBpmBound
        {
            get { return lowerBpmBound; }
        }

        public double UpperBpmBound
        {
            get { return upperBpmBound; }
        }

        public BpmRange(int degrees, double lowerBpmBound, double upperBpmBound)
        {
            this.degrees = degrees;
            this.lowerBpmBound = lowerBpmBound;
            this.upperBpmBound = upperBpmBound;
        }

        public static implicit operator int(BpmRange range)
        {
            return range.Degrees;
        }

        public bool Equals(BpmRange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Degrees == other.Degrees;
        }

        public int CompareTo(BpmRange other)
        {
            return Degrees.CompareTo(other.Degrees);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BpmRange) obj);
        }

        public override int GetHashCode()
        {
            return Degrees;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((BpmRange)obj);
        }

        public override string ToString()
        {
            return String.Format("{0:0.##} - {1:0.##}", lowerBpmBound, upperBpmBound);
        }
    }
}