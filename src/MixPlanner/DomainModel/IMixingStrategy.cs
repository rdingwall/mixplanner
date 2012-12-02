namespace MixPlanner.DomainModel
{
    public interface IMixingStrategy
    {
        bool IsCompatible(Track firstTrack, Track secondTrack);
        string Description { get; }
    }
}