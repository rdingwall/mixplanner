using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface ICompatibilityFactorCalculator
    {
        double CalculateCompatibilityFactor(MixItem current, Track next);
    }

    public class CompatibilityFactorCalculator : ICompatibilityFactorCalculator
    {
        readonly IDictionary<IMixingStrategy, double> mixingStrategyFactors;

        public CompatibilityFactorCalculator(IEnumerable<IMixingStrategy> preferredStrategies)
        {
            if (preferredStrategies == null) throw new ArgumentNullException("preferredStrategies");

            var strategiesList = preferredStrategies.ToList();

            // Factor is simply the strategy's index divided by number of
            // strategies. E.g. favourite strategy is 1, next is a bit lower,
            // etc. Note we use count + 1 so that it doesn't return zero for
            // the last strategy. (Zero is reserved, it means "not compatible
            // at all").
            Func<IMixingStrategy, IList<IMixingStrategy>, double> getFactor =
                (s, strategies) => 1 - (double) strategies.IndexOf(s)/(strategies.Count + 1);

            mixingStrategyFactors = strategiesList
                .ToDictionary(k => k, v => getFactor(v, strategiesList));
        }

        public double CalculateCompatibilityFactor(MixItem current, Track next)
        {
            if (current == null) throw new ArgumentNullException("current");
            if (next == null) throw new ArgumentNullException("next");

            var nextPlaybackSpeed = next.GetDefaultPlaybackSpeed();

            return mixingStrategyFactors
                .Where(p => p.Key.IsCompatible(current.PlaybackSpeed, nextPlaybackSpeed))
                .Select(p => p.Value)
                .FirstOrDefault();
        }
    }
}