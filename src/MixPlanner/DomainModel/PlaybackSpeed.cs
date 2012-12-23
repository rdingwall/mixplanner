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
        }

        public void SetSpeed(double percentIncrease)
        {
            PercentIncrease = percentIncrease;
            ActualBpm = CalculateActualBpm(percentIncrease);
            ActualKey = CalculateActualKey(percentIncrease);
        }

        HarmonicKey CalculateActualKey(double percentIncrease)
        {
            var pitchIncrease = 7*(percentIncrease/3);
            return originalKey.IncreasePitch((int)pitchIncrease);
        }

        double CalculateActualBpm(double percentIncrease)
        {
            var increase = percentIncrease / 100;
            return originalBpm * (1 + increase);
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

        public double PercentIncrease { get; private set; }
        public double ActualBpm { get; private set; }
        public HarmonicKey ActualKey { get; private set; }
    }
}