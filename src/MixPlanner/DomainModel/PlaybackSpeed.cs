using System;

namespace MixPlanner.DomainModel
{
    public class PlaybackSpeed
    {
        public PlaybackSpeed(HarmonicKey originalKey, double originalBpm)
        {
            if (originalKey == null) throw new ArgumentNullException("originalKey");
            this.originalBpm = originalBpm;
            this.originalKey = originalKey;
            ActualBpm = originalBpm;
            ActualKey = originalKey;
            Speed = 1;
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
            var percentIncrease = (speed - 1)*100;

            // Pitch changes one semitone (+/- 7 on the Camelot wheel) for
            // every 3% difference.
            var pitchIncrease = 7*(percentIncrease/3);

            return originalKey.IncreasePitch((int)pitchIncrease);
        }

        double CalculateActualBpm(double speed)
        {
            return originalBpm * speed;
        }

        public bool IsWithinBpmRange(PlaybackSpeed other)
        {
            if (other == null) throw new ArgumentNullException("other");

            var difference = other.ActualBpm - ActualBpm;
            var percentIncreaseRequired = difference/ActualBpm * 100;
            return Math.Abs(percentIncreaseRequired) < 3;
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
    }
}