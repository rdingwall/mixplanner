using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestPlaybackSpeeds
    {
        public static PlaybackSpeed Get(double bpm)
        {
            return new PlaybackSpeed(HarmonicKey.Key6B, HarmonicKey.Key6B, bpm);
        }

        public static PlaybackSpeed Starting(HarmonicKey key, double bpm)
        {
            return new PlaybackSpeed(key, key, bpm);
        }

        public static PlaybackSpeed Get(
            HarmonicKey startingKey, HarmonicKey endingKey, double bpm)
        {
            return new PlaybackSpeed(startingKey, endingKey, bpm);
        }
    }
}