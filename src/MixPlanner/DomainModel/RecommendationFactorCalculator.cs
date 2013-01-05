using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IRecommendationFactorCalculator
    {
        double GetRecommendationFactor(MixItem current, Track next);
    }

    public class RecommendationFactorCalculator : IRecommendationFactorCalculator
    {
        readonly IDictionary<IMixingStrategy, double> mixingStrategyFactors;

        public RecommendationFactorCalculator(IEnumerable<IMixingStrategy> preferredStrategies)
        {
            if (preferredStrategies == null) throw new ArgumentNullException("preferredStrategies");

            var strategiesList = preferredStrategies.ToList();

            // Factor is simply the strategy's index divided by number of strategies.
            Func<IMixingStrategy, IList<IMixingStrategy>, double> getFactor =
                (s, strategies) => 1 - (double) strategies.IndexOf(s)/(strategies.Count + 1);

            mixingStrategyFactors = strategiesList
                .ToDictionary(k => k, v => getFactor(v, strategiesList));
        }

        public double GetRecommendationFactor(MixItem current, Track next)
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