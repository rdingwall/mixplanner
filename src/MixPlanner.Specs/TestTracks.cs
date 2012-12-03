using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestTracks
    {
        static readonly Random Random = new Random();

        public static Track Get(HarmonicKey key, float bpm)
        {
            return new Track("Test Artist",
                key.ToString(),
                key,
                string.Format("{0:N}.mp3", Guid.NewGuid()),
                bpm);
        }

        public static Track Get(HarmonicKey key)
        {
            return Get(key, GetRandomBpm());
        }

        public static float GetRandomBpm()
        {
            const float max = 150;
            const float min = 120;
            var factor = (float)Random.NextDouble();

            return factor*(max - min) + min;
        }
    }
}