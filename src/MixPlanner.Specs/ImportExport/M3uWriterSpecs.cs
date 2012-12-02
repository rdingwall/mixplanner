using System.IO;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.ImportExport;

namespace MixPlanner.Specs.ImportExport
{
    [Subject(typeof(M3uWriter))]
    public class M3uWriterSpecs
    {
         public class When_exporting_an_m3u_file
         {
             Establish context =
                 () =>
                     {
                         writer = new M3uWriter();
                         path = Path.GetTempFileName();
                         mix = TestMixes.GetRandomMix();
                     };

             Because of = () => writer.Write(mix, path);

             It should_write_a_file = 
                 () => File.ReadAllLines(path).ShouldContainOnly(mix.Tracks.Select(t => t.File.FullName));

             Cleanup after = () =>
                                 {
                                     if (File.Exists(path))
                                         File.Delete(path);
                                 };

             static M3uWriter writer;
             static string path;
             static Mix mix;
         }
    }
}