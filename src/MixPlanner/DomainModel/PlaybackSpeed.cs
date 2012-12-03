using System;

namespace MixPlanner.DomainModel
{
    public class PlaybackSpeed
    {
        public PlaybackSpeed(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            originalBpm = track.OriginalBpm;
            originalKey = track.OriginalKey;
            ActualBpm = track.OriginalBpm;
            ActualKey = track.OriginalKey;
        }

        public PlaybackSpeed(HarmonicKey originalKey, double originalBpm)
        {
            if (originalKey == null) throw new ArgumentNullException("originalKey");
            this.originalBpm = originalBpm;
            this.originalKey = originalKey;
            ActualBpm = originalBpm;
            ActualKey = originalKey;
        }

        public void SetSpeed(int percentIncrease)
        {
            PercentIncrease = percentIncrease;
            ActualBpm = CalculateActualBpm(percentIncrease);
            ActualKey = CalculateActualKey(percentIncrease);
        }

        HarmonicKey CalculateActualKey(int percentIncrease)
        {
            var pitchIncrease = 7*(percentIncrease/3);
            return originalKey.IncreasePitch(pitchIncrease);
        }

        double CalculateActualBpm(int percentIncrease)
        {
            var increase = (double)percentIncrease / 100;
            return originalBpm * (1 + increase);
        }

        readonly double originalBpm;
        readonly HarmonicKey originalKey;

        public int PercentIncrease { get; private set; }
        public double ActualBpm { get; private set; }
        public HarmonicKey ActualKey { get; private set; }
    }
}