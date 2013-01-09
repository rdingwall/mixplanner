using System;

namespace MixPlanner.ViewModels
{
    public class TrackLibrarySearchFilter
    {
        readonly string searchText;
        const StringComparison Comparison = StringComparison.CurrentCultureIgnoreCase;

        public TrackLibrarySearchFilter(string searchText)
        {
            if (searchText == null) throw new ArgumentNullException("searchText");
            this.searchText = searchText;
        }

        public bool Filter(object obj)
        {
            var item = (TrackLibraryItemViewModel)obj;

            return item.SearchData.IndexOf(searchText, Comparison) > -1;
        }
    }
}