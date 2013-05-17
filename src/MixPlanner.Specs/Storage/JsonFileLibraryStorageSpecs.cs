using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Loader;
using MixPlanner.Storage;
using SharpTestsEx;
using System.Linq;

namespace MixPlanner.Specs.Storage
{
    [Subject(typeof(JsonFileLibraryStorage))]
    public class JsonFileLibraryStorageSpecs
    {
         public class When_saving_updating_and_loading_all_the_tracks
         {
             Establish context =
                 () =>
                     {
                         TestLibraryDir.Recreate();

                         storage = new JsonFileLibraryStorage(new TrackImageResizer(), TestLibraryDir.DirectoryName);

                         trackA = TestTracks.CreateRandomTrack();
                         trackB = TestTracks.CreateRandomTrack();

                         originalTracks = new[] {trackA, trackB};

                         Task.WaitAll(
                             storage.AddTrackAsync(trackA),
                             storage.AddTrackAsync(trackB));

                         trackA.Artist = "different artist";
                         storage.UpdateTrackAsync(trackA).Wait();

                         var goodImageFilename = Path.Combine(TestLibraryDir.DirectoryName,
                                                              String.Format("{0}.png", trackA.Id));
                         File.Copy("example.png", goodImageFilename);

                         TestLibraryDir.Touch("corrupt.track");

                         var corruptFilename = String.Format("{0}.png", trackB.Id);
                         TestLibraryDir.CreateFile(corruptFilename, "this is a corrupt png");
                     };

             Cleanup after = () => TestLibraryDir.Delete();

             Because of = () =>
                              {
                                  tracks = storage.LoadAllTracksAsync().Result;
                                  loadedTrackA = tracks.FirstOrDefault(t => t.Id == trackA.Id);
                                  loadedTrackB = tracks.FirstOrDefault(t => t.Id == trackB.Id);
                              };

             It should_load_all_the_tracks = () => tracks.Should().Have.SameValuesAs(originalTracks);

             It should_correctly_retrieve_the_tracks_id = () => loadedTrackA.Id.ShouldEqual(trackA.Id);
             It should_correctly_retrieve_the_tracks_title = () => loadedTrackA.Title.ShouldEqual(trackA.Title);
             It should_correctly_retrieve_the_tracks_artist = () => loadedTrackA.Artist.ShouldEqual(trackA.Artist);
             It should_correctly_retrieve_the_tracks_key = () => loadedTrackA.OriginalKey.ShouldEqual(trackA.OriginalKey);
             It should_correctly_retrieve_the_tracks_bpm = () => loadedTrackA.OriginalBpm.ShouldEqual(trackA.OriginalBpm);
             It should_correctly_retrieve_the_tracks_filename = () => loadedTrackA.Filename.ShouldEqual(trackA.Filename);
             It should_correctly_retrieve_the_tracks_images = () => loadedTrackA.HasImages.ShouldBeTrue();

             It should_skip_corrupt_images = () => loadedTrackB.HasImages.ShouldBeFalse();

             static IEnumerable<Track> tracks;
             static ILibraryStorage storage;
             static Track track;
             static Track trackA;
             static Track trackB;
             static IEnumerable<Track> originalTracks;
             static Track loadedTrackA;
             static Track loadedTrackB;
         }
    }
}