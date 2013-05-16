using System;
using System.Collections.Generic;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Loader;
using MixPlanner.Storage;

namespace MixPlanner.Specs.Storage
{
    [Subject(typeof(JsonFileLibraryStorage))]
    public class JsonFileLibraryStorageSpecs
    {
         public class When_trying_to_load_tracks
         {
             Establish context = () => storage = new JsonFileLibraryStorage(new TrackImageResizer(), "\\nonexistent\foo");

             Because of = () => exception = Catch.Exception(() => tracks = storage.FetchAllAsync().Result);

             It should_not_throw_any_exception = () => exception.ShouldBeNull();

             It should_return_an_empty_collection = () => tracks.ShouldBeEmpty();

             static IEnumerable<Track> tracks;
             static Exception exception;
             static ILibraryStorage storage;
         }
    }
}