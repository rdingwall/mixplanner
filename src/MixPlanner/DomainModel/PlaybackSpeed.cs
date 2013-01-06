using System;

namespace MixPlanner.DomainModel
{
    public class PlaybackSpeed : IEquatable<PlaybackSpeed>, ICloneable
    {
        private const double DefaultSpeed = 1;

        // The harmonic key changes with every +/-3% pitch change.
        public const double HarmonicKeyChangeInterval = 0.03;

        public PlaybackSpeed(
            HarmonicKey originalKey, 
            double originalBpm, 
            double speed = DefaultSpeed)
        {
            if (originalKey == null) throw new ArgumentNullException("originalKey");
            this.originalBpm = originalBpm;
            this.originalKey = originalKey;
            ActualBpm = originalBpm;
            ActualKey = originalKey;
            SetSpeed(speed);
        }

        public void SetSpeed(double speed)
        {
            Speed = speed;
            ActualBpm = CalculateActualBpm(speed);
            ActualKey = CalculateActualKey(speed);
        }

        HarmonicKey CalculateActualKey(double speed)
        {
            // How many percentage points we are increasing the speed
            // e.g. 1.03 = 103% = 3
            //      0.94 =  -6% = 6
            var percentIncrease = (speed - 1) * 100;

            // Pitch changes one semitone (+/- 7 on the Camelot wheel) for
            // every 3% difference.
            var pitchIncrease = 7 * (percentIncrease / 3);

            return originalKey.IncreasePitch((int)pitchIncrease);
        }

        double CalculateActualBpm(double speed)
        {
            return originalBpm * speed;
        }

        public bool IsWithinBpmRange(PlaybackSpeed other)
        {
            if (other == null) throw new ArgumentNullException("other");

            var increaseRequired = GetExactIncreaseRequiredToMatch(other);
            return Math.Abs(increaseRequired) < HarmonicKeyChangeInterval;
        }

        public double GetExactIncreaseRequiredToMatch(PlaybackSpeed other)
        {
            if (other == null) throw new ArgumentNullException("other");
            var difference = other.ActualBpm - ActualBpm;
            return difference / ActualBpm;
        }

        readonly double originalBpm;
        readonly HarmonicKey originalKey;

        public double Speed { get; private set; }
        public double ActualBpm { get; private set; }
        public HarmonicKey ActualKey { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} ({1}, {2})", Speed, ActualBpm, ActualKey);
        }

        public void Reset()
        {
            SetSpeed(DefaultSpeed);
        }

        public bool Equals(PlaybackSpeed other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ActualBpm.Equals(other.ActualBpm) && Equals(ActualKey, other.ActualKey) && Speed.Equals(other.Speed) && originalBpm.Equals(other.originalBpm) && Equals(originalKey, other.originalKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaybackSpeed)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ActualBpm.GetHashCode();
                hashCode = (hashCode * 397) ^ (ActualKey != null ? ActualKey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Speed.GetHashCode();
                hashCode = (hashCode * 397) ^ originalBpm.GetHashCode();
                hashCode = (hashCode * 397) ^ (originalKey != null ? originalKey.GetHashCode() : 0);
                return hashCode;
            }
        }

        public object Clone()
        {
            return new PlaybackSpeed(originalKey, originalBpm, Speed);
        }

        public void Increase(double amount)
        {
            SetSpeed(DefaultSpeed + amount);
        }

        public PlaybackSpeed AsIncreasedBy(double amount)
        {
            var increased = (PlaybackSpeed) Clone();
            increased.Increase(amount);
            return increased;
        }
    }
}