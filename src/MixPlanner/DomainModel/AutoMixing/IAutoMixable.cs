namespace MixPlanner.DomainModel.AutoMixing
{
    /// <summary>
    /// Interface required to use auto mixing (you can automix MixItems or
    /// unplayed tracks).
    /// </summary>
    public interface IAutoMixable
    {
        PlaybackSpeed PlaybackSpeed { get; }
        bool IsUnknownKeyOrBpm { get; }
    }
}