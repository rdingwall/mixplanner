using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public static class GroupByBpmRangeExtensions
    {
         public static IEnumerable<IGrouping<BpmRange, IMixItem>> GroupByBpmRange(
             this IEnumerable<IMixItem> items)
         {
             return items
                 .GroupBy(g => BpmRangeCalculator.GetNumberOfDegreesDistanceFromMean(g.ActualBpm))
                 .OrderByDescending(g => g.Count())
                 .ToList();
         }
    }
}