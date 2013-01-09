using System;

namespace MixPlanner.DomainModel
{
    public interface IPlaybackSpeed : IEquatable<IPlaybackSpeed>, ICloneable
    {
        void SetSpeed(double speed);
        bool IsWithinBpmRange(IPlaybackSpeed other);
        double GetExactIncreaseRequiredToMatch(IPlaybackSpeed other);
        double Speed { get; }
        double ActualBpm { get; }
        HarmonicKey ActualKey { get; }
        void Reset();
        void Increase(double amount);
        IPlaybackSpeed AsIncreasedBy(double amount);
    }

}