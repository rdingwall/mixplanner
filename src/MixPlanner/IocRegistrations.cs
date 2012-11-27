using System.Windows;
using System.Windows.Input;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Mp3;
using MixPlanner.Storage;
using MixPlanner.ViewModels;

namespace MixPlanner
{
    public class IocRegistrations : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITrackLibrary>().ImplementedBy<InMemoryTrackLibrary>(),
                Component.For<ITrackLoader>().ImplementedBy<TrackLoader>(),
                Component.For<IId3Reader>().ImplementedBy<Id3Reader>(),
                Component.For<IMessenger>().ImplementedBy<Messenger>(),
                AllTypes.FromThisAssembly().InSameNamespaceAs<MainWindowViewModel>(),
                AllTypes.FromThisAssembly().BasedOn<Window>(),
                AllTypes.FromThisAssembly().BasedOn<ICommand>());
        }
    }
}