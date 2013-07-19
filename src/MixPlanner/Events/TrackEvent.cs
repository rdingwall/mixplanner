using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public abstract class TrackEvent
    {
        public Track Track { get; protected set; }
    }
}