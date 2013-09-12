using System;
using System.IO;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Specs.Extensions;
using MixPlanner.Storage;
using Rhino.Mocks;
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

                                         var messenger = MockRepository.GenerateStub<IDispatcherMessenger>();
                                         var library = MockRepository.GenerateStub<ITrackLibrary>();
                                         storage = new JsonFileMixStorage(
                                             new MixFactory(messenger,
                                                            TestMixes.TransitionDetector,
                                                            TestMixes.PlaybackSpeedAdjuster),
                                             library);
                                     };

             Because of = () =>
                              {
                                  originalMix.Dump("expected mix");

                                  filename = Path.Combine(TestDirectories.Data.Path,
                                                          String.Format("{0:N}.mix", Guid.NewGuid()));
                                  storage.SaveAsync(originalMix, filename).Wait();
                                  mix = storage.OpenAsync(filename).Result;

                                  mix.Dump("actual mix");
                              };

             It should_have_the_same_track_ids_in_the_same_sequence =
                 () => mix.Select(i => i.Track.Id).Should().Have.SameSequenceAs(originalMix.Select(i => i.Track.Id));

             It should_have_the_same_track_keys_in_the_same_sequence =
                 () => mix.Select(i => i.ActualKey).Should().Have.SameSequenceAs(originalMix.Select(i => i.ActualKey));

             It should_have_the_same_tracks_in_the_same_sequence = 
                 () => mix.ShouldHaveSameSequenceAs(originalMix, new MixItemAssertComparer());


             static IMix originalMix;
             static IMix mix;
             static IMixStorage storage;
             static string filename;
         }
    }
}