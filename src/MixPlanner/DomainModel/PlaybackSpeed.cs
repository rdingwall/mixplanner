using System;

namespace MixPlanner.DomainModel
{
    public class PlaybackSpeed
    {
        public PlaybackSpeed(Track track)
        {
            PercentIncrease = 0;
            OriginalBpm = track.OriginalBpm;
            OriginalKey = track.OriginalKey;
            ActualBpm = track.OriginalBpm;
            ActualKey = track.OriginalKey;
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
            var key = OriginalKey.IncreasePitch(pitchIncrease);
            return key;
        }

        float CalculateActualBpm(int percentIncrease)
        {
            var increase = (float)percentIncrease / 100;
            return OriginalBpm * (1 + increase);
        }

        protected float OriginalBpm { get; private set; }
        protected HarmonicKey OriginalKey { get; private set; }
        public int PercentIncrease { get; private set; }
        public double ActualBpm { get; private set; }
        public HarmonicKey ActualKey { get; private set; }
        
    }
}