using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlayerStoppedEvent : TrackEvent
    {
        public PlayerStoppedEvent(Track track)
        {
            Track = track;
        }
    }
}