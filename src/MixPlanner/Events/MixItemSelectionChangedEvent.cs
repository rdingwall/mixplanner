using System;
using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class MixItemSelectionChangedEvent
    {
        public MixItemSelectionChangedEvent(IEnumerable<IMixItem> selectedItems)
        {
            if (selectedItems == null) throw new ArgumentNullException("selectedItems");
            SelectedItems = selectedItems;
        }

        public IEnumerable<IMixItem> SelectedItems { get; private set; } 
    }
}