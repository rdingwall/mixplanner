using System;
using MixPlanner.DomainModel;
using MixPlanner.IO.Loader;

namespace MixPlanner.Specs
{
    public static class TestPlaybackSpeeds
    {
        public static PlaybackSpeed Create(double bpm)
        {
            return new PlaybackSpeed(HarmonicKey.Key6B, bpm);
        }

        public static PlaybackSpeed Random()
        {
            return new PlaybackSpeed(HarmonicKey.Key6B, TestTracks.GetRandomBpm());
        }

        public static PlaybackSpeed Create(HarmonicKey key)
        {
            return new PlaybackSpeed(key, TestTracks.GetRandomBpm());
        }
    }

    public static class TestTracks
    {
        static readonly Random Random = new Random();

        public static Track CreateRandomTrack(HarmonicKey key, double bpm)
        {
            return new Track(artist: String.Format("Artist-{0:N}", Guid.NewGuid()),
                             title: String.Format("Title-{0:N}", Guid.NewGuid()),
                             originalKey: key,
                             fileName: String.Format("{0:N}.mp3", Guid.NewGuid()),
                             originalBpm: bpm,
                             duration: GetRandomDuration())
                       {
                           Genre = String.Format("Genre-{0}", Random.Next()),
                           Year = Random.Next(1980, 2013).ToString(),
                           Label = String.Format("Label-{0}", Random.Next())
                       };
        }

        public static Track CreateTrackWithFilenameOnly(string filename)
        {
            return new Track(TrackDefaults.UnknownArtist,
                             TrackDefaults.UnknownTitle,
                             HarmonicKey.Unknown,
                             filename,
                             double.NaN,
                             GetRandomDuration())
                       {
                           Genre = String.Format("Genre-{0}", Random.Next()),
                           Year = Random.Next(1980, 2013).ToString(),
                           Label = String.Format("Label-{0}", Random.Next())
                       };
        }

        public static Track CreateRandomTrack()
        {
            return CreateRandomTrack(HarmonicKey.RandomKey(), GetRandomBpm());
        }

        public static Track CreateRandomTrack(HarmonicKey key)
        {
            return CreateRandomTrack(key, GetRandomBpm());
        }

        public static Track CreateRandomTrack(double bpm)
        {
            return CreateRandomTrack(HarmonicKey.RandomKey(), bpm);
        }

        public static double GetRandomBpm()
        {
            const double maxBpm = 150;
            const double minBpm = 120;
            var factor = Random.NextDouble();

            return factor * (maxBpm - minBpm) + minBpm;
        }

        public static TimeSpan GetRandomDuration()
        {
            return new TimeSpan(hours: 0, minutes: Random.Next(8), seconds: Random.Next(60));
        }
    }
}