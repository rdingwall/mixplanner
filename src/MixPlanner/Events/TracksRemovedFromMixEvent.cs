using System;
using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TracksRemovedFromMixEvent
    {
        public IEnumerable<IMixItem> Items { get; private set; }

        public TracksRemovedFromMixEvent(IEnumerable<IMixItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            Items = items;
        }
    }
}