using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public class DummyPlaybackSpeedAdjuster : IPlaybackSpeedAdjuster
    {
        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            return 0;
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second)
        {
            return second;
        }
    }
}