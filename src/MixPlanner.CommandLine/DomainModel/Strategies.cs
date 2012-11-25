using System.Collections.Generic;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.CommandLine.DomainModel
{
    public class Strategies
    {
        // Strategies in order of most-to-least exciting
        public static IEnumerable<IMixingStrategy> PreferredOrder
            = new List<IMixingStrategy>
                  {
                      new TwoSemitoneEnergyBoost(),
                      new SameKey(),
                      new OneSemitoneEnergyBoost(),
                      new IncrementOne(),
                      new SwitchToMajorScale(),
                      new SwitchToMinorScale()
                  };

        public static IEnumerable<IMixingStrategy> AllStrategies
            = new List<IMixingStrategy>
                  {
                      new TwoSemitoneEnergyBoost(),
                      new SameKey(),
                      new OneSemitoneEnergyBoost(),
                      new IncrementOne(),
                      new SwitchToMajorScale(),
                      new SwitchToMinorScale(),
                      new ManualOutOfKeyMix()
                  };
    }
}