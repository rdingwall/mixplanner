using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public class DummyPlaybackSpeedAdjuster : ILimitingPlaybackSpeedAdjuster
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