using System;
using System.Diagnostics;

namespace MixPlanner.DomainModel
{
    [DebuggerDisplay("{ActualStartingKey} - {ActualEndingKey} ({ActualBpm})")]
    public class PlaybackSpeed
    {
        public PlaybackSpeed(
            HarmonicKey originalStartingKey,
            HarmonicKey originalEndingingKey,
            double originalBpm)
        {
            if (originalStartingKey == null) throw new ArgumentNullException("originalStartingKey");
            if (originalEndingingKey == null) throw new ArgumentNullException("originalEndingingKey");
            this.originalBpm = originalBpm;
            this.originalStartingKey = originalStartingKey;
            this.originalEndingingKey = originalEndingingKey;
            ActualBpm = originalBpm;
            ActualStartingKey = originalStartingKey;
            ActualEndingKey = originalEndingingKey;
        }

        public void SetSpeed(double percentIncrease)
        {
            PercentIncrease = percentIncrease;
            ActualBpm = CalculateActualBpm(percentIncrease);
            ActualStartingKey = CalculateActualKey(originalStartingKey, percentIncrease);
            ActualEndingKey = CalculateActualKey(originalEndingingKey, percentIncrease);
        }

        static HarmonicKey CalculateActualKey(HarmonicKey key, double percentIncrease)
        {
            var pitchIncrease = 7*(percentIncrease/3);
            return key.IncreasePitch((int)pitchIncrease);
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
        readonly HarmonicKey originalStartingKey;
        readonly HarmonicKey originalEndingingKey;

        public double PercentIncrease { get; private set; }
        public double ActualBpm { get; private set; }
        public HarmonicKey ActualStartingKey { get; private set; }
        public HarmonicKey ActualEndingKey { get; private set; }
    }
}