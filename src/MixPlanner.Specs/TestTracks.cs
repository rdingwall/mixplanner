using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestTracks
    {
        static readonly Random Random = new Random();

        public static Track Get(HarmonicKey key, double bpm)
        {
            return new Track("Test Artist",
                key.ToString(),
                key,
                string.Format("{0:N}.mp3", Guid.NewGuid()),
                bpm);
        }

        public static Track GetRandomTrack()
        {
            return Get(HarmonicKey.RandomKey(), GetRandomBpm());
        }

        public static Track Get(HarmonicKey key)
        {
            return Get(key, GetRandomBpm());
        }

        public static double GetRandomBpm()
        {
            const double maxBpm = 150;
            const double minBpm = 120;
            var factor = Random.NextDouble();

            return factor * (maxBpm - minBpm) + minBpm;
        }

        public static PlaybackSpeed PlaybackSpeed(double bpm)
        {
            return new PlaybackSpeed(HarmonicKey.Key6B, bpm);
        }
    }
}