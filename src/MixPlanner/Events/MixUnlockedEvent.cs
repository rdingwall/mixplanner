using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class MixUnlockedEvent
    {
        public MixUnlockedEvent(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            Mix = mix;
        }

        public IMix Mix { get; private set; }
    }
}