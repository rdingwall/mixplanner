namespace MixPlanner.DomainModel.AutoMixing
{
    /// <summary>
    /// Interface required to use auto mixing (you can automix MixItems or
    /// unplayed tracks).
    /// </summary>
    public interface IAutoMixable
    {
        HarmonicKey ActualKey { get; }
        bool IsUnknownKeyOrBpm { get; }
    }
}