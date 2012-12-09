using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlayerStoppedEvent
    {
        public PlayerStoppedEvent(Track track)
        {
            Track = track;
        }

        public Track Track { get; private set; }
    }
}