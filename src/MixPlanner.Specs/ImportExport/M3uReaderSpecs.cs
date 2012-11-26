using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.ImportExport;
using MixPlanner.Mp3;

namespace MixPlanner.Specs.ImportExport
{
    [Subject(typeof(M3uReader))]
    public class M3uReaderSpecs
    {
         public class When_reading_an_m3u_file
         {
             Establish context = () =>
                                     {
                                         filename = "DummyPlaylist.m3u";
                                         reader = new M3uReader(new TrackLoader(new Id3Reader()));
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