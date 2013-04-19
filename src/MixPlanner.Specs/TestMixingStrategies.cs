﻿using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;
using System.Linq;

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
                                    new PerfectFifth(bpmRangeChecker),
                                    new RelativeMajor(bpmRangeChecker),
                                    new RelativeMinor(bpmRangeChecker)
                                };

            AllStrategies = new IMixingStrategy[]
                                {
                                    new TwoSemitoneEnergyBoost(bpmRangeChecker),
                                    new SameKey(bpmRangeChecker),
                                    new OneSemitoneEnergyBoost(bpmRangeChecker),
                                    new PerfectFifth(bpmRangeChecker),
                                    new RelativeMajor(bpmRangeChecker),
                                    new RelativeMinor(bpmRangeChecker),
                                    new ManualOutOfKeyMix(bpmRangeChecker),
                                    new ManualIncompatibleBpmsMix(bpmRangeChecker) 
                                };

            AllCompatibleStrategies = PreferredStrategies.Concat(
                new IMixingStrategy[]
                    {
                        new OneSemitoneDecrease(bpmRangeChecker),
                        new TwoSemitoneDecrease(bpmRangeChecker),
                        new PerfectFourth(bpmRangeChecker) 
                    });
        }

        public static IEnumerable<IMixingStrategy> AllCompatibleStrategies { get; private set; }

        public static IMixingStrategiesFactory CreateFactory()
        {
            using (var container = new WindsorContainer())
            {
                container.Register(Component.For<IBpmRangeChecker>().ImplementedBy<AlwaysInRangeBpmChecker>(),
                                   AllTypes.FromAssemblyContaining<SameKey>()
                                           .BasedOn<IMixingStrategy>()
                                           .WithServiceSelf());

                return new MixingStrategiesFactory(container);
            }
        }

        public static IEnumerable<IMixingStrategy> AllStrategies { get; private set; }
        public static IEnumerable<IMixingStrategy> PreferredStrategies { get; private set; }
    }
}