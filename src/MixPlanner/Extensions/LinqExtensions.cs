using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace System.Linq
// ReSharper restore CheckNamespace
{
    public static class LinqExtensions
    {
        static readonly Random Random = new Random();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return items.OrderBy(_ => Random.Next());
        }
    }
}