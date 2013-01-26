using System;
using System.Collections.Generic;
using Castle.Windsor;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.DomainModel
{
    public interface IMixingStrategiesFactory
    {
        IEnumerable<IMixingStrategy> GetStrategiesInPreferredOrder();
        IEnumerable<IMixingStrategy> GetAllStrategies();
    }

    public class MixingStrategiesFactory : IMixingStrategiesFactory
    {
        readonly IWindsorContainer container;

        public MixingStrategiesFactory(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        // Strategies in order of most-to-least exciting
        public IEnumerable<IMixingStrategy> GetStrategiesInPreferredOrder()
        {
            return new IMixingStrategy[]
                       {
                           container.Resolve<TwoSemitoneEnergyBoost>(),
                           container.Resolve<SameKey>(),
                           container.Resolve<OneSemitone>(),
                           container.Resolve<PerfectFifth>(),
                           container.Resolve<RelativeMajor>(),
                           container.Resolve<RelativeMinor>()
                       };
        }

        public IEnumerable<IMixingStrategy> GetAllStrategies()
        {
            return new IMixingStrategy[]
                       {
                           container.Resolve<TwoSemitoneEnergyBoost>(),
                           container.Resolve<SameKey>(),
                           container.Resolve<OneSemitone>(),
                           container.Resolve<PerfectFifth>(),
                           container.Resolve<PerfectFourth>(),
                           container.Resolve<RelativeMajor>(),
                           container.Resolve<RelativeMinor>(),
                           container.Resolve<ManualOutOfKeyMix>(),
                           container.Resolve<ManualIncompatibleBpmsMix>()
                       };
        }
    }
}