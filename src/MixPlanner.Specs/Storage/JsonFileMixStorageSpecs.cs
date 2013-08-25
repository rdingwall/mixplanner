using System;
using System.IO;
using System.Linq;
using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.Storage;
using SharpTestsEx;

namespace MixPlanner.Specs.Storage
{
    [Subject(typeof(JsonFileMixStorage))]
    public class JsonFileMixStorageSpecs
    {
         public class When_saving_and_opening_a_mix
         {
             Establish context = () =>
                                     {
                                         originalMix = TestMixes.CreateRandomMix(10);

                                         container = new WindsorContainer();
                                         container.Install(new MixPlannerWindsorInstaller());
                                         container.Resolve<IConfigProvider>().InitializeAsync().Wait();
                                         storage = container.Resolve<IMixStorage>();
                                     };

             Because of = () =>
                              {
                                  filename = Path.Combine(TestDirectories.Data.Path,
                                                          String.Format("{0:N}.mix", Guid.NewGuid()));
                                  storage.SaveAsync(originalMix, filename).Wait();
                                  mix = storage.OpenAsync(filename).Result;
                              };

             It should_have_the_same_track_ids_in_the_same_sequence =
                 () => mix.Select(i => i.Track.Id).Should().Have.SameSequenceAs(originalMix.Select(i => i.Track.Id));

             It should_have_the_same_track_keys_in_the_same_sequence =
                 () => mix.Select(i => i.ActualKey).Should().Have.SameSequenceAs(originalMix.Select(i => i.ActualKey));


             It should_have_the_same_tracks_in_the_same_sequence = 
                 () => mix.ShouldHaveSameSequenceAs(originalMix, new MixItemAssertComparer());

             //It should_have_the_same_tracks_in_the_same_sequence_equals =
               //  () => mix.Should().Have.SameSequenceAs(originalMix);

             Cleanup after = () => container.Dispose();

             static IMix originalMix;
             static IMix mix;
             static IMixStorage storage;
             static string filename;
             static IWindsorContainer container;
         }
    }
}