using System;
using System.Collections.Generic;
using System.Linq;
using SharpTestsEx;

// ReSharper disable CheckNamespace
namespace MixPlanner.Specs
// ReSharper restore CheckNamespace
{
    public static class AssertExtensions
    {
         public static void ShouldHaveSameSequenceAs<T>(this IEnumerable<T> actual, IEnumerable<T> expected,
                                                        IEqualityComparer<T> comparer)
         {
             if (actual == null) throw new ArgumentNullException("actual");
             if (expected == null) throw new ArgumentNullException("expected");
             if (comparer == null) throw new ArgumentNullException("comparer");
             actual.Should().Have.Count.EqualTo(expected.Count());
             actual.Zip(expected, comparer.Equals).ToArray();
         }
    }
}