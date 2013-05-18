using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Castle.Core;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Configuration;
using MixPlanner.Controllers;
using MixPlanner.Converters;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using MixPlanner.Loader;
using MixPlanner.Player;
using MixPlanner.ProgressDialog;
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
            container.AddFacility<StartableFacility>(f => f.DeferredStart());

            const string preferredStrategies = "PreferredStrategies";
            const string allStrategies = "AllStrategies";

            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<ITrackLibrary>().ImplementedBy<TrackLibrary>(),
                Component.For<ITrackLoader>().ImplementedBy<TrackLoader>(),
                Component.For<ITagCleanupFactory>().ImplementedBy<TagCleanupFactory>(),
                Component.For<IFilenameParser>().ImplementedBy<ConfigSwitchingFilenameParser>()
                    .DependsOn(Property.ForKey("impl").Is(typeof(FilenameParser).Name)),
                Component.For<IFilenameParser>().ImplementedBy<FilenameParser>().Named(typeof(FilenameParser).Name),
                Component.For<ILibraryStorage>().ImplementedBy<JsonFileLibraryStorage>(),
                Component.For<IConfigStorage>().ImplementedBy<JsonFileConfigStorage>(),
                Component.For<IConfigProvider>().ImplementedBy<ConfigProvider>(),
                Component.For<IHarmonicKeySuperParser>().ImplementedBy<HarmonicKeySuperParser>(),
                Component.For<IId3Reader>().ImplementedBy<Id3Reader>(),
                Component.For<IAiffId3Reader>().ImplementedBy<AiffId3Reader>(),
                Component.For<IAacReader>().ImplementedBy<AacReader>(),
                Component.For<ITrackImageResizer>().ImplementedBy<TrackImageResizer>(),
                Component.For<IMessenger>().ImplementedBy<Messenger>(),
                Component.For<IDispatcherMessenger>().ImplementedBy<DispatcherMessenger>(),
                Component.For<IAudioPlayer>().ImplementedBy<AudioPlayer>(),
                    Component.For<IBpmRangeChecker>().ImplementedBy<ConfigSwitchingBpmRangeChecker>()
                    .DependsOn(Property.ForKey("impl").Is(typeof(BpmRangeChecker).Name)),
                Component.For<IBpmRangeChecker>().ImplementedBy<BpmRangeChecker>()
                    .Named(typeof(BpmRangeChecker).Name),
                Component.For<ILimitingPlaybackSpeedAdjuster>().ImplementedBy<ConfigSwitchingPlaybackSpeedAdjuster>()
                    .DependsOn(Property.ForKey("impl").Is(typeof(PlaybackSpeedAdjuster).Name)),
                Component.For<ILimitingPlaybackSpeedAdjuster>().ImplementedBy<LimitingPlaybackSpeedAdjuster>()
                    .Named(typeof(PlaybackSpeedAdjuster).Name),
                Component.For<IPlaybackSpeedAdjuster>().ImplementedBy<PlaybackSpeedAdjuster>(),
                Component.For<IEdgeCostCalculator>().ImplementedBy<EdgeCostCalculator>(),
                Component.For<IHarmonicKeyConverterFactory>().ImplementedBy<HarmonicKeyConverterFactory>(),
                Component.For<IMixingStrategiesFactory>().ImplementedBy<MixingStrategiesFactory>(),
                Component.For<IMixItemViewModelFactory>().ImplementedBy<MixItemViewModelFactory>(),
                Component.For<IActualTransitionDetector>().ImplementedBy<ActualTransitionDetector>()
                         .DependsOn(Property.ForKey("strategies").Is(allStrategies)),
                Component.For<IRecommendedTransitionDetector>().ImplementedBy<RecommendedTransitionDetector>()
                         .DependsOn(Property.ForKey("preferredstrategies").Is(preferredStrategies)),
                Component.For<IAutoMixingContextFactory>().ImplementedBy<AutoMixingContextFactory>(),
                Component.For<IAutoMixingStrategy>().ImplementedBy<AutoMixingStrategy>(),
                Component.For<IMix>().ImplementedBy<Mix>(),
                AllTypes.FromThisAssembly().InSameNamespaceAs<MainWindowViewModel>()
                    .ConfigureFor<SettingsWindowViewModel>(c => c.LifestyleTransient()),
                AllTypes.FromThisAssembly().BasedOn<Window>().LifestyleTransient(),
                AllTypes.FromThisAssembly().BasedOn<ICommand>(),
                AllTypes.FromThisAssembly().BasedOn<IValueConverter>().WithServiceSelf(),
                AllTypes.FromThisAssembly().BasedOn<IMixingStrategy>().WithServiceSelf(),
                Component.For<IEnumerable<IMixingStrategy>>().Named(preferredStrategies)
                         .UsingFactoryMethod(k => k.Resolve<IMixingStrategiesFactory>().GetPreferredStrategiesInOrder()),
                Component.For<IEnumerable<IMixingStrategy>>().Named(allStrategies)
                         .UsingFactoryMethod(k => k.Resolve<IMixingStrategiesFactory>().GetAllStrategies()),
                Component.For<IProgressDialogService>().ImplementedBy<ProgressDialogService>(),
                Component.For<RecommendationsController>().Start());
        }
    }
}