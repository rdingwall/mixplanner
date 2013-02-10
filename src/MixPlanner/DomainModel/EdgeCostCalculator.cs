using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IEdgeCostCalculator
    {
        EdgeCost CalculateCost(PlaybackSpeed previous, PlaybackSpeed next);
    }

    public class EdgeCostCalculator : IEdgeCostCalculator
    {
        readonly IEnumerable<IMixingStrategy> preferredStrategies;
        readonly IEnumerable<IMixingStrategy> nonPreferredStrategies;
        readonly IPlaybackSpeedAdjuster adjuster;

        public EdgeCostCalculator(
            IPlaybackSpeedAdjuster adjuster, 
            IMixingStrategiesFactory strategies)
        {
            if (adjuster == null) throw new ArgumentNullException("adjuster");
            if (strategies == null) throw new ArgumentNullException("strategies");
            this.adjuster = adjuster;
            preferredStrategies = strategies.GetPreferredStrategiesInOrder();
            nonPreferredStrategies = strategies.GetNonPreferredCompatibleStrategies();
        }

        public EdgeCost CalculateCost(PlaybackSpeed previous, PlaybackSpeed next)
        {
            if (previous == null && next == null)
                throw new ArgumentNullException("previous", "At least one playback speed is required.");

            if (previous == null)
                return new EdgeCost(2);

            if (next == null)
                return new EdgeCost(2);

            var nextAdjusted = adjuster.AutoAdjust(previous, next);
            IMixingStrategy strategy;

            double cost = 0;
            cost += CalculateBpmCost(nextAdjusted.Adjustment);
            cost += CalculateHarmonicCost(previous, nextAdjusted, out strategy);

            return new EdgeCost(cost, previous, nextAdjusted, strategy, next.Adjustment);
        }

        static double CalculateBpmCost(double requiredIncrease)
        {
            if (Math.Abs(requiredIncrease) < PitchFaderStep.Value)
                return 0;

            if (Math.Abs(requiredIncrease) < PitchFaderStep.Value*2)
                return 5;

            if (Math.Abs(requiredIncrease) < PitchFaderStep.Value*3)
                return 10;

            return 1000;
        }

        double CalculateHarmonicCost(PlaybackSpeed previous, PlaybackSpeed nextAdjusted, out IMixingStrategy strategy)
        {
            if ((strategy = GetPreferredStrategy(previous, nextAdjusted)) != null)
                return 0;

            if ((strategy = GetFallbackStrategy(previous, nextAdjusted)) != null)
                return 10;

            return 100;
        }

        IMixingStrategy GetFallbackStrategy(PlaybackSpeed previous, PlaybackSpeed nextAdjusted)
        {
            return nonPreferredStrategies.FirstOrDefault(s => s.IsCompatible(previous, nextAdjusted));
        }

        IMixingStrategy GetPreferredStrategy(PlaybackSpeed previous, PlaybackSpeed nextAdjusted)
        {
            return preferredStrategies.FirstOrDefault(s => s.IsCompatible(previous, nextAdjusted));
        }
    }
}