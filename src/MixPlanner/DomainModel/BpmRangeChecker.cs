using System;

namespace MixPlanner.DomainModel
{
    public interface IBpmRangeChecker
    {
        bool IsWithinBpmRange(PlaybackSpeed first, PlaybackSpeed second);
    }

    public class BpmRangeChecker : IBpmRangeChecker
    {
         public bool IsWithinBpmRange(PlaybackSpeed first, PlaybackSpeed second)
         {
             if (first == null) throw new ArgumentNullException("first");
             if (second == null) throw new ArgumentNullException("second");

             return first.IsWithinBpmRange(second);
         }
    }
}