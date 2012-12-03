using System;

namespace MixPlanner.DomainModel
{
    public class PlaybackSpeed
    {
        public PlaybackSpeed(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            OriginalBpm = track.OriginalBpm;
            OriginalKey = track.OriginalKey;
            ActualBpm = track.OriginalBpm;
            ActualKey = track.OriginalKey;
        }

        public PlaybackSpeed(float originalBpm, HarmonicKey originalKey)
        {
            if (originalKey == null) throw new ArgumentNullException("originalKey");
            OriginalBpm = originalBpm;
            OriginalKey = originalKey;
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
            return OriginalKey.IncreasePitch(pitchIncrease);
        }

        float CalculateActualBpm(int percentIncrease)
        {
            var increase = (float)percentIncrease / 100;
            return OriginalBpm * (1 + increase);
        }

        public float OriginalBpm { get; private set; }
        public HarmonicKey OriginalKey { get; private set; }
        public int PercentIncrease { get; private set; }
        public double ActualBpm { get; private set; }
        public HarmonicKey ActualKey { get; private set; }
    }
}