using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Castle.Windsor;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;
using NUnit.Framework;

namespace MixPlanner.Specs
{
    public class IocRegistrationsSpecs
    {
        [TestFixture]
        public class When_resolving_view_models
        {
            [TestFixtureSetUp]
            public void SetUp()
            {
                container = new WindsorContainer();
                container.Install(new IocRegistrations());
            }

            [TestFixtureTearDown]
            public void TearDown()
            {
                container.Dispose();
            }

            [Test]
            public void It_should_be_able_to_resolve_the_main_window_view_model()
            {
                Assert.That(container.Resolve<MainWindowViewModel>(), Is.Not.Null);
            }

            [Test, TestCaseSource("GetCommandTypes")]
            public void It_should_be_able_to_resolve_the_command(Type type)
            {
                Assert.That(container.Resolve(type), Is.Not.Null);
            }

            [Test, TestCaseSource("GetMixingStrategyTypes")]
            public void It_should_be_able_to_resolve_the_mixing_strategy(Type type)
            {
                Assert.That(container.Resolve(type), Is.Not.Null);
            }

            [Test]
            public void It_should_be_able_to_resolve_the_preferred_mixing_strategies()
            {
                var strategies = container.Resolve<IEnumerable<IMixingStrategy>>("PreferredStrategies");
                Assert.That(strategies, Is.Not.Empty);
            }

            [Test]
            public void It_should_be_able_to_resolve_all_mixing_strategies()
            {
                var strategies = container.Resolve<IEnumerable<IMixingStrategy>>("AllStrategies");
                Assert.That(strategies, Is.Not.Empty);
            }

            static IEnumerable<Type> GetCommandTypes()
            {
                return typeof (AddTrackToMixCommand)
                    .Assembly.GetTypes()
                    .Where(t => typeof (ICommand).IsAssignableFrom(t))
                    .Where(t => !t.IsAbstract)
                    .Except(new[]
                                {
                                    typeof (PlayPauseTrackCommand)
                                });
            }

            static IEnumerable<Type> GetMixingStrategyTypes()
            {
                return typeof (IMixingStrategy)
                    .Assembly.GetTypes()
                    .Where(t => typeof (IMixingStrategy).IsAssignableFrom(t))
                    .Where(t => !t.IsAbstract);
            }

            static IWindsorContainer container;
        }
    }
}