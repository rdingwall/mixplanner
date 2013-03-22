using System;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class EdgeCost : IEquatable<EdgeCost>, IComparable<EdgeCost>, IComparable
    {
        public double Cost { get; private set; }
        public PlaybackSpeed PreviousPlaybackSpeed { get; private set; }
        public PlaybackSpeed NextPlaybackSpeed { get; private set; }
        public IMixingStrategy MixingStrategy { get; private set; }
        public double IncreaseRequired { get; private set; }

        public EdgeCost(
            double cost, 
            PlaybackSpeed previousPlaybackSpeed = null, 
            PlaybackSpeed nextPlaybackSpeed = null, 
            IMixingStrategy mixingStrategy = null, 
            double increaseRequired = 0)
        {
            Cost = cost;
            PreviousPlaybackSpeed = previousPlaybackSpeed;
            NextPlaybackSpeed = nextPlaybackSpeed;
            MixingStrategy = mixingStrategy;
            IncreaseRequired = increaseRequired;
        }

        public bool Equals(EdgeCost other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Cost.Equals(other.Cost) && Equals(PreviousPlaybackSpeed, other.PreviousPlaybackSpeed) && Equals(NextPlaybackSpeed, other.NextPlaybackSpeed) && Equals(MixingStrategy, other.MixingStrategy) && IncreaseRequired.Equals(other.IncreaseRequired);
        }

        public int CompareTo(EdgeCost other)
        {
            if (other == null)
                return 1;

            if (Cost == other.Cost)
                return 0;

            return Cost - other.Cost > 0 ? 1 : 0;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EdgeCost) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Cost.GetHashCode();
                hashCode = (hashCode*397) ^ (PreviousPlaybackSpeed != null ? PreviousPlaybackSpeed.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (NextPlaybackSpeed != null ? NextPlaybackSpeed.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (MixingStrategy != null ? MixingStrategy.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IncreaseRequired.GetHashCode();
                return hashCode;
            }
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as EdgeCost);
        }

        public override string ToString()
        {
            return Cost.ToString();
        }
    }
}