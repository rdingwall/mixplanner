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
                           container.Resolve<OneSemitoneEnergyBoost>(),
                           container.Resolve<IncrementOne>(),
                           container.Resolve<SwitchToMajorScale>(),
                           container.Resolve<SwitchToMinorScale>()
                       };
        }

        public IEnumerable<IMixingStrategy> GetAllStrategies()
        {
            return new IMixingStrategy[]
                       {
                           container.Resolve<TwoSemitoneEnergyBoost>(),
                           container.Resolve<SameKey>(),
                           container.Resolve<OneSemitoneEnergyBoost>(),
                           container.Resolve<IncrementOne>(),
                           container.Resolve<SwitchToMajorScale>(),
                           container.Resolve<SwitchToMinorScale>(),
                           container.Resolve<ManualOutOfKeyMix>(),
                           container.Resolve<ManualIncompatibleBpmsMix>()
                       };
        }
    }
}