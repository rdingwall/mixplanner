namespace MixPlanner.DomainModel
{
    public interface IMixingStrategy
    {
        bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second);
        string Description { get; }
    }
}