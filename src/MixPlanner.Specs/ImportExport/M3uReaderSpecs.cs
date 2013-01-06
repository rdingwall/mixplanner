﻿using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.ImportExport;
using MixPlanner.Mp3;
using Rhino.Mocks;

namespace MixPlanner.Specs.ImportExport
{
    [Subject(typeof(M3uReader))]
    public class M3uReaderSpecs
    {
         public class When_reading_an_m3u_file
         {
             Establish context = () =>
                                     {
                                         var cleanupFactory = MockRepository
                                             .GenerateMock<IId3TagCleanupFactory>();
                                         cleanupFactory.Stub(f => f.GetCleanups())
                                                       .Return(new IId3TagCleanup[0]);

                                         filename = "DummyPlaylist.m3u";
                                         reader = new M3uReader(new TrackLoader(new Id3Reader(),
                                             cleanupFactory));
                                     };

             Because of = () => tracks = reader.Read(filename);

             It should_read_all_the_title_names =
                 () => tracks.Select(t => t.Title).ShouldContainOnly("Aaa", "Bbb", "Ccc");

             It should_read_all_the_file_names =
                () => tracks.Select(t => t.File.Name).ShouldContainOnly("Aaa.mp3", "Bbb.mp3", "Ccc.mp3");

             static M3uReader reader;
             static string filename;
             static IEnumerable<Track> tracks;
         }
    }
}