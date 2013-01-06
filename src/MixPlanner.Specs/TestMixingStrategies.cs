using System.Collections.Generic;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs
{
    public static class TestMixingStrategies
    {
        static TestMixingStrategies()
        {
            var bpmRangeChecker = new AlwaysInRangeBpmChecker();

            PreferredStrategies = new IMixingStrategy[]
                                {
                                    new TwoSemitoneEnergyBoost(bpmRangeChecker),
                                    new SameKey(bpmRangeChecker),
                                    new OneSemitoneEnergyBoost(bpmRangeChecker),
                                    new AdvanceOne(bpmRangeChecker),
                                    new SwitchToMajorScale(bpmRangeChecker),
                                    new SwitchToMinorScale(bpmRangeChecker)
                                };

            AllStrategies = new IMixingStrategy[]
                                {
                                    new TwoSemitoneEnergyBoost(bpmRangeChecker),
                                    new SameKey(bpmRangeChecker),
                                    new OneSemitoneEnergyBoost(bpmRangeChecker),
                                    new AdvanceOne(bpmRangeChecker),
                                    new SwitchToMajorScale(bpmRangeChecker),
                                    new SwitchToMinorScale(bpmRangeChecker),
                                    new ManualOutOfKeyMix(bpmRangeChecker),
                                    new ManualIncompatibleBpmsMix(bpmRangeChecker) 
                                };
        }

        public static IEnumerable<IMixingStrategy> AllStrategies { get; private set; }
        public static IEnumerable<IMixingStrategy> PreferredStrategies { get; private set; }
    }
}