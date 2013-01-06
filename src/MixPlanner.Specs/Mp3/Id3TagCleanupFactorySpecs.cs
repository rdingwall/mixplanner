using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Mp3;
using Rhino.Mocks;

namespace MixPlanner.Specs.Mp3
{
    [Subject(typeof(Id3TagCleanupFactory))]
    public class Id3TagCleanupFactorySpecs
    {
         public class When_mik_prefix_cleanups_were_enabled
         {
             Establish context =
                 () =>
                     {
                         var provider = MockRepository.GenerateMock<IConfigurationProvider>();
                         provider.Stub(c => c.Configuration)
                             .Return(new Configuration { StripMixedInKeyPrefixes = true });

                         cleanupFactory = new Id3TagCleanupFactory(provider);
                     };

             Because of = () => cleanups = cleanupFactory.GetCleanups();

             It should_return_an_id3_cleanup = 
                 () => cleanups.Single().ShouldBe(typeof(MixedInKeyTagCleanup));

             static IId3TagCleanupFactory cleanupFactory;
             static IEnumerable<IId3TagCleanup> cleanups;
         }

         public class When_mik_prefix_cleanups_were_not_enabled
         {
             Establish context =
                 () =>
                 {
                     var provider = MockRepository.GenerateMock<IConfigurationProvider>();
                     provider.Stub(c => c.Configuration).Return(new Configuration());

                     cleanupFactory = new Id3TagCleanupFactory(provider);
                 };

             Because of = () => cleanups = cleanupFactory.GetCleanups();

             It should_not_return_any_cleanups =
                 () => cleanups.ShouldBeEmpty();

             static IId3TagCleanupFactory cleanupFactory;
             static IEnumerable<IId3TagCleanup> cleanups;
         }
    }
}