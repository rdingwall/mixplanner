using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(MixingStrategiesFactory))]
    public class MixingStrategiesFactorySpecs
    {
         public class When_getting_non_preferred_strategies
         {
             Establish context = () =>
                                     {
                                         container = new WindsorContainer();
                                         container.Install(new IocRegistrations());
                                         factory = new MixingStrategiesFactory(container);
                                     };

             static IWindsorContainer container;
             static IMixingStrategiesFactory factory;

             Because of = () => strategies = factory.GetNonPreferredCompatibleStrategies();

             Cleanup after = () => container.Dispose();

             static IEnumerable<IMixingStrategy> strategies;

             It should_return_the_expected_strategies =
                 () => strategies.Select(s => s.GetType())
                                 .ShouldContainOnly(
                                     typeof (PerfectFourth),
                                     typeof (TwoSemitoneDecrease),
                                     typeof (OneSemitoneDecrease));
         }
    }
}