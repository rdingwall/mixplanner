using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Configuration;
using MixPlanner.Converters;
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

            const string preferredStrategies = "PreferredStrategies";
            const string allStrategies = "AllStrategies";

            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<ITrackLibrary>().ImplementedBy<TrackLibrary>(),
                Component.For<ITrackLoader>().ImplementedBy<TrackLoader>(),
                Component.For<IId3TagCleanupFactory>().ImplementedBy<Id3TagCleanupFactory>(),
                Component.For<ILibraryStorage>().ImplementedBy<InMemoryLibraryStorage>(),
                Component.For<IConfigStorage>().ImplementedBy<InMemoryConfigStorage>(),
                Component.For<IConfigProvider>().ImplementedBy<ConfigProvider>(),
                Component.For<IId3Reader>().ImplementedBy<Id3Reader>(),
                Component.For<IMessenger>().ImplementedBy<Messenger>(),
                Component.For<IDispatcherMessenger>().ImplementedBy<DispatcherMessenger>(),
                Component.For<IAudioPlayer>().ImplementedBy<AudioPlayer>(),
                Component.For<IBpmRangeChecker>().ImplementedBy<BpmRangeChecker>(),
                Component.For<IHarmonicKeyConverterFactory>().ImplementedBy<HarmonicKeyConverterFactory>(),
                Component.For<IMixingStrategiesFactory>().ImplementedBy<MixingStrategiesFactory>(),
                Component.For<IMixItemViewModelFactory>().ImplementedBy<MixItemViewModelFactory>(),
                Component.For<IRelaxedTransitionDetector>().ImplementedBy<RelaxedTransitionDetector>()
                         .DependsOn(Property.ForKey("strategies").Is(allStrategies)),
                Component.For<IStrictTransitionDetector>().ImplementedBy<StrictTransitionDetector>()
                         .DependsOn(Property.ForKey("preferredstrategies").Is(preferredStrategies)),
                Component.For<IMix>().ImplementedBy<Mix>(),
                AllTypes.FromThisAssembly().InSameNamespaceAs<MainWindowViewModel>()
                    .ConfigureFor<SettingsWindowViewModel>(c => c.LifestyleTransient()),
                AllTypes.FromThisAssembly().BasedOn<Window>().LifestyleTransient(),
                AllTypes.FromThisAssembly().BasedOn<ICommand>(),
                AllTypes.FromThisAssembly().BasedOn<IValueConverter>().WithServiceSelf(),
                AllTypes.FromThisAssembly().BasedOn<IMixingStrategy>().WithServiceSelf(),
                Component.For<IEnumerable<IMixingStrategy>>().Named(preferredStrategies)
                         .UsingFactoryMethod(k => k.Resolve<IMixingStrategiesFactory>().GetStrategiesInPreferredOrder()),
                Component.For<IEnumerable<IMixingStrategy>>().Named(allStrategies)
                         .UsingFactoryMethod(k => k.Resolve<IMixingStrategiesFactory>().GetAllStrategies()));
        }
    }
}