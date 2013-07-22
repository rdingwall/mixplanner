using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

// ReSharper disable CheckNamespace
namespace GongSolutions.Wpf.DragDrop
// ReSharper restore CheckNamespace
{
    public static class DropInfoExtensions
    {
        public static IEnumerable<T> DataAsEnumerable<T>(this IDropInfo dropInfo)
        {
            if (dropInfo.Data is T)
                return new [] {(T)dropInfo.Data};

            var items = dropInfo.Data as IEnumerable<T>;
            if (items != null)
                return items;

            return Enumerable.Empty<T>();
        }

        public static Track GetTrack(this IDropInfo dropInfo)
        {
            var mixItems = dropInfo.Data as IEnumerable<IMixItem>;
            if (mixItems != null)
                return mixItems.Select(i => i.Track).FirstOrDefault();

            var libraryItemViewModels = dropInfo.Data as IEnumerable<TrackLibraryItemViewModel>;
            if (libraryItemViewModels != null)
                return libraryItemViewModels.Select(v => v.Track).FirstOrDefault();

            return dropInfo.Data as Track;
        }
    }
}