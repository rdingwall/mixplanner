using System;

namespace MixPlanner.DomainModel
{
    public interface IMixingStrategy : IEquatable<IMixingStrategy>
    {
        bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second);
        string Description { get; }
    }
}