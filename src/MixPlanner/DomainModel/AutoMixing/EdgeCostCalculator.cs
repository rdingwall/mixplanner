using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public interface IEdgeCostCalculator
    {
        double CalculateCost(IMixingStrategy strategy);
    }

    public class EdgeCostCalculator : IEdgeCostCalculator
    {
        readonly HashSet<IMixingStrategy> preferredStrategies;
        readonly HashSet<IMixingStrategy> nonPreferredStrategies;

        public EdgeCostCalculator(
            IMixingStrategiesFactory strategies)
        {
            if (strategies == null) throw new ArgumentNullException("strategies");
            preferredStrategies = new HashSet<IMixingStrategy>(strategies.GetPreferredStrategiesInOrder());
            nonPreferredStrategies = new HashSet<IMixingStrategy>(strategies.GetNonPreferredCompatibleStrategies());
        }

        public double CalculateCost(IMixingStrategy strategy)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");

            if (preferredStrategies.Contains(strategy))
                return 0;

            if (nonPreferredStrategies.Contains(strategy))
                return 10;

            return 100;
        }
    }
}