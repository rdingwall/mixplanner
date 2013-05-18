using System.Collections.Generic;
using System.Linq;

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
    }
}