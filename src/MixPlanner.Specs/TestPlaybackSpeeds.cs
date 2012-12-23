using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestPlaybackSpeeds
    {
        public static PlaybackSpeed Get(double bpm)
        {
            return new PlaybackSpeed(HarmonicKey.RandomKey(), HarmonicKey.Key6B, bpm);
        }

        public static PlaybackSpeed Ending(HarmonicKey key, double bpm)
        {
            return new PlaybackSpeed(HarmonicKey.RandomKey(), key, bpm);
        }

        public static PlaybackSpeed Starting(HarmonicKey key, double bpm)
        {
            return new PlaybackSpeed(key, HarmonicKey.RandomKey(), bpm);
        }

        public static PlaybackSpeed Get(
            HarmonicKey startingKey, HarmonicKey endingKey, double bpm)
        {
            return new PlaybackSpeed(startingKey, endingKey, bpm);
        }
    }
}