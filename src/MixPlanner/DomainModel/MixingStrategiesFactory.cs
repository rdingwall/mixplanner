using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.DomainModel
{
    public interface IMixingStrategiesFactory
    {
        IEnumerable<IMixingStrategy> GetPreferredStrategiesInOrder();
        IEnumerable<IMixingStrategy> GetAllStrategies();
        IEnumerable<IMixingStrategy> GetNonPreferredCompatibleStrategies();
        IEnumerable<IMixingStrategy> GetIncompatibleStrategies();
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
        public IEnumerable<IMixingStrategy> GetPreferredStrategiesInOrder()
        {
            return new IMixingStrategy[]
                       {
                           container.Resolve<TwoSemitoneEnergyBoost>(),
                           container.Resolve<SameKey>(),
                           container.Resolve<OneSemitoneEnergyBoost>(),
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
                           container.Resolve<TwoSemitoneDecrease>(),
                           container.Resolve<SameKey>(),
                           container.Resolve<OneSemitoneEnergyBoost>(),
                           container.Resolve<OneSemitoneDecrease>(),
                           container.Resolve<PerfectFifth>(),
                           container.Resolve<PerfectFourth>(),
                           container.Resolve<RelativeMajor>(),
                           container.Resolve<RelativeMinor>(),
                           container.Resolve<ManualOutOfKeyMix>(),
                           container.Resolve<UnknownTransition>(), // more specific, must come first
                           container.Resolve<ManualIncompatibleBpmsMix>()
                       };
        }

        public IEnumerable<IMixingStrategy> GetNonPreferredCompatibleStrategies()
        {
            return new IMixingStrategy[]
                       {
                           container.Resolve<TwoSemitoneDecrease>(),
                           container.Resolve<OneSemitoneDecrease>(),
                           container.Resolve<PerfectFourth>()
                       };
        }

        public IEnumerable<IMixingStrategy> GetIncompatibleStrategies()
        {
            return new IMixingStrategy[]
                       {
                           container.Resolve<ManualOutOfKeyMix>(),
                           container.Resolve<UnknownTransition>(), // more specific, must come first
                           container.Resolve<ManualIncompatibleBpmsMix>()
                       };
        }
    }
}