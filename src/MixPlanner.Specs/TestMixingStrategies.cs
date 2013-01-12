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
                                    new OneSemitone(bpmRangeChecker),
                                    new PerfectFifth(bpmRangeChecker),
                                    new RelativeMajor(bpmRangeChecker),
                                    new RelativeMinor(bpmRangeChecker)
                                };

            AllStrategies = new IMixingStrategy[]
                                {
                                    new TwoSemitoneEnergyBoost(bpmRangeChecker),
                                    new SameKey(bpmRangeChecker),
                                    new OneSemitone(bpmRangeChecker),
                                    new PerfectFifth(bpmRangeChecker),
                                    new RelativeMajor(bpmRangeChecker),
                                    new RelativeMinor(bpmRangeChecker),
                                    new ManualOutOfKeyMix(bpmRangeChecker),
                                    new ManualIncompatibleBpmsMix(bpmRangeChecker) 
                                };
        }

        public static IEnumerable<IMixingStrategy> AllStrategies { get; private set; }
        public static IEnumerable<IMixingStrategy> PreferredStrategies { get; private set; }
    }
}