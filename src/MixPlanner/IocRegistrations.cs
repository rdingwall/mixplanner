using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Mp3;
using MixPlanner.Player;
using MixPlanner.Storage;
using MixPlanner.ViewModels;

namespace MixPlanner
{
    public class IocRegistrations : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<ITrackLibrary>().ImplementedBy<TrackLibrary>(),
                Component.For<ITrackLoader>().ImplementedBy<TrackLoader>()
                         .DependsOn(Property.ForKey("cleanups")),
                Component.For<ILibraryStorage>().ImplementedBy<InMemoryLibraryStorage>(),
                Component.For<IId3Reader>().ImplementedBy<Id3Reader>(),
                Component.For<IMessenger>().ImplementedBy<Messenger>(),
                Component.For<IAudioPlayer>().ImplementedBy<AudioPlayer>(),
                Component.For<IBpmRangeChecker>().ImplementedBy<BpmRangeChecker>(),
                Component.For<IMixingStrategiesFactory>().ImplementedBy<MixingStrategiesFactory>(),
                Component.For<IMixItemViewModelFactory>().ImplementedBy<MixItemViewModelFactory>(),
                Component.For<ITransitionDetector>().ImplementedBy<TransitionDetector>()
                         .DependsOn(Property.ForKey("strategies").Is("AllStrategies")),
                Component.For<IMix>().ImplementedBy<Mix>(),
                AllTypes.FromThisAssembly().InSameNamespaceAs<MainWindowViewModel>(),
                AllTypes.FromThisAssembly().BasedOn<Window>(),
                AllTypes.FromThisAssembly().BasedOn<IId3TagCleanup>().WithServiceBase(),
                AllTypes.FromThisAssembly().BasedOn<ICommand>(),
                AllTypes.FromThisAssembly().BasedOn<IMixingStrategy>().WithServiceSelf(),
                Component.For<IEnumerable<IMixingStrategy>>().Named("PreferredStrategies")
                         .UsingFactoryMethod(k => k.Resolve<IMixingStrategiesFactory>().GetStrategiesInPreferredOrder()),
                Component.For<IEnumerable<IMixingStrategy>>().Named("AllStrategies")
                         .UsingFactoryMethod(k => k.Resolve<IMixingStrategiesFactory>().GetAllStrategies()));
        }
    }
}