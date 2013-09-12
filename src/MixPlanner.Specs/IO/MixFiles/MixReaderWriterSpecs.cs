using System;
using System.IO;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.IO.MixFiles;
using MixPlanner.Specs.Extensions;
using Rhino.Mocks;
using SharpTestsEx;

namespace MixPlanner.Specs.IO.MixFiles
{
    [Subject("Mix reader/writer")]
    public class MixReaderWriterSpecs
    {
         public class When_saving_and_opening_a_mix
         {
             Establish context = () =>
                                     {
                                         originalMix = TestMixes.CreateRandomMix(10);

                                         var messenger = MockRepository.GenerateStub<IDispatcherMessenger>();
                                         var library = MockRepository.GenerateStub<ITrackLibrary>();

                                         writer = new MixWriter();

                                         reader = new MixReader(
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
                                  writer.WriteAsync(originalMix, filename).Wait();
                                  mix = reader.ReadAsync(filename).Result;

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
             static IMixWriter writer;
             static string filename;
             static IMixReader reader;
         }
    }
}