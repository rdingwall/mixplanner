using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class MixSavedEvent
    {
        public MixSavedEvent(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            Mix = mix;
        }

        public IMix Mix { get; private set; }
    }
}