using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.ViewModels;

namespace MixPlanner.Specs
{
    [Subject(typeof(IocRegistrations))]
    public class IocRegistrationsSpecs
    {
         public class When_resolving_view_models
         {
             Establish context = () => container = new WindsorContainer();

             Because of = () => container.Install(new IocRegistrations());

             It should_be_able_to_resolve_the_main_window_view_model =
                 () => container.Resolve<MainWindowViewModel>();

             Cleanup after = () => container.Dispose();

             static IWindsorContainer container; 
         }
    }
}