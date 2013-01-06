using System;

namespace MixPlanner.DomainModel
{
    public class Transition : IEquatable<Transition>, IComparable<Transition>, IComparable
    {
        public static Transition Intro(HarmonicKey toKey)
        {
            if (toKey == null) throw new ArgumentNullException("toKey");
            return new Transition { ToKey = toKey };
        }

        public static Transition Outro(HarmonicKey fromKey)
        {
            if (fromKey == null) throw new ArgumentNullException("fromKey");
            return new Transition { FromKey = fromKey };
        }

        private Transition() { }

        public Transition(
            HarmonicKey fromKey,
            HarmonicKey toKey,
            IMixingStrategy strategy,
            double increaseRequired = 0)
        {
            if (fromKey == null) throw new ArgumentNullException("fromKey");
            if (toKey == null) throw new ArgumentNullException("toKey");
            if (strategy == null) throw new ArgumentNullException("strategy");
            FromKey = fromKey;
            ToKey = toKey;
            Strategy = strategy;
            IncreaseRequired = increaseRequired;
        }

        public HarmonicKey FromKey { get; private set; }
        public HarmonicKey ToKey { get; private set; }
        public IMixingStrategy Strategy { get; private set; }
        public double IncreaseRequired { get; private set; }

        string Description
        {
            get { return Strategy != null ? Strategy.Description : ""; }
        }

        public bool Equals(Transition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ToKey, other.ToKey) && Equals(FromKey, other.FromKey) && Equals(Strategy, other.Strategy) && IncreaseRequired.Equals(other.IncreaseRequired);
        }

        public int CompareTo(Transition other)
        {
            if (other == null)
                return 1;

            // Sort by strategy descriptions if there are any
            return Description.CompareTo(other.Description);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Transition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Transition)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ToKey != null ? ToKey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FromKey != null ? FromKey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Strategy != null ? Strategy.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IncreaseRequired.GetHashCode();
                return hashCode;
            }
        }
    }
}