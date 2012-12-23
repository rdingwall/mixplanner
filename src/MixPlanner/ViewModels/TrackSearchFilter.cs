using System;

namespace MixPlanner.ViewModels
{
    public class TrackSearchFilter
    {
        readonly string searchText;
        const StringComparison Comparison = StringComparison.CurrentCultureIgnoreCase;

        public TrackSearchFilter(string searchText)
        {
            if (searchText == null) throw new ArgumentNullException("searchText");
            this.searchText = searchText;
        }

        public bool Filter(object obj)
        {
            var item = (LibraryItemViewModel)obj;

            return item.Track.SearchData.IndexOf(searchText, Comparison) > -1;
        }
    }
}