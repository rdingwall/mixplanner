using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public class AlwaysInRangeBpmChecker : IBpmRangeChecker
    {
        public bool IsWithinBpmRange(PlaybackSpeed first, PlaybackSpeed second)
        {
            return true;
        }
    }
}