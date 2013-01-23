using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.Loader;
using Rhino.Mocks;

namespace MixPlanner.Specs.Loader
{
    [Subject(typeof(TagCleanupFactory))]
    public class TagCleanupFactorySpecs
    {
         public class When_mik_prefix_cleanups_were_enabled
         {
             Establish context =
                 () =>
                     {
                         var provider = MockRepository.GenerateMock<IConfigProvider>();
                         provider.Stub(c => c.Config)
                             .Return(new Config { StripMixedInKeyPrefixes = true });

                         cleanupFactory = new TagCleanupFactory(provider);
                     };

             Because of = () => cleanups = cleanupFactory.GetCleanups();

             It should_return_an_id3_cleanup = 
                 () => cleanups.Single().ShouldBe(typeof(MixedInKeyTagCleanup));

             static ITagCleanupFactory cleanupFactory;
             static IEnumerable<ITagCleanup> cleanups;
         }

         public class When_mik_prefix_cleanups_were_not_enabled
         {
             Establish context =
                 () =>
                 {
                     var provider = MockRepository.GenerateMock<IConfigProvider>();
                     provider.Stub(c => c.Config).Return(new Config());

                     cleanupFactory = new TagCleanupFactory(provider);
                 };

             Because of = () => cleanups = cleanupFactory.GetCleanups();

             It should_not_return_any_cleanups =
                 () => cleanups.ShouldBeEmpty();

             static ITagCleanupFactory cleanupFactory;
             static IEnumerable<ITagCleanup> cleanups;
         }
    }
}