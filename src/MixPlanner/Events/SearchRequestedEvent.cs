using System;

namespace MixPlanner.Events
{
    public class SearchRequestedEvent
    {
        public string SearchText { get; set; }

        public SearchRequestedEvent(string searchText)
        {
            if (searchText == null) throw new ArgumentNullException("searchText");
            SearchText = searchText;
        }
    }
}