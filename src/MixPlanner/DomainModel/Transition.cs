using System;

namespace MixPlanner.DomainModel
{
    public class Transition : IEquatable<Transition>, IComparable<Transition>, IComparable
    {
        public static Transition Intro(HarmonicKey toKey)
        {
            return new Transition { ToKey = toKey, IsIntro = true };
        }

        public bool IsIntro { get; private set; }
        public bool IsOutro { get; private set; }

        public static Transition Outro(HarmonicKey fromKey)
        {
            return new Transition { FromKey = fromKey, IsOutro = true };
        }

        private Transition() { }

        public Transition(
            HarmonicKey fromKey,
            HarmonicKey toKey,
            IMixingStrategy strategy,
            double increaseRequired = 0)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");
            FromKey = fromKey;
            ToKey = toKey;
            Strategy = strategy;
            IncreaseRequired = increaseRequired;
        }

        public HarmonicKey? FromKey { get; private set; }
        public HarmonicKey? ToKey { get; private set; }
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
            return IsIntro.Equals(other.IsIntro) && IsOutro.Equals(other.IsOutro) && FromKey.Equals(other.FromKey) && ToKey.Equals(other.ToKey) && Equals(Strategy, other.Strategy) && IncreaseRequired.Equals(other.IncreaseRequired);
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
            return Equals((Transition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsIntro.GetHashCode();
                hashCode = (hashCode*397) ^ IsOutro.GetHashCode();
                hashCode = (hashCode*397) ^ FromKey.GetHashCode();
                hashCode = (hashCode*397) ^ ToKey.GetHashCode();
                hashCode = (hashCode*397) ^ (Strategy != null ? Strategy.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IncreaseRequired.GetHashCode();
                return hashCode;
            }
        }
    }
}