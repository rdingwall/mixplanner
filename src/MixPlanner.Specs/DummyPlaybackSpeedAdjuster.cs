using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public class DummyPlaybackSpeedAdjuster : ILimitingPlaybackSpeedAdjuster
    {
        public double CalculateSuggestedIncrease(PlaybackSpeed track, double targetBpm)
        {
            return 0;
        }

        public double CalculateSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            return 0;
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second)
        {
            return second;
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed track, double targetBpm)
        {
            throw new System.NotImplementedException();
        }

        public void AutoAdjustAll(IEnumerable<PlaybackSpeed> tracks, double targetBpm)
        {
            throw new System.NotImplementedException();
        }
    }
}