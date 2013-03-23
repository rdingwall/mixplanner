using System;

namespace MixPlanner.DomainModel
{
    public interface IMixingStrategy : IEquatable<IMixingStrategy>
    {
        bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second);
        bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey);
        string Description { get; }
    }
}