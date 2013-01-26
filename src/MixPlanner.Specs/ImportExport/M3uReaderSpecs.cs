using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Machine.Specifications;
using MixPlanner.Converters;
using MixPlanner.DomainModel;
using MixPlanner.ImportExport;
using MixPlanner.Loader;
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
                                            .GenerateMock<ITagCleanupFactory>();
                                        cleanupFactory.Stub(f => f.GetCleanups())
                                                      .Return(new ITagCleanup[0]);

                                        var resizer = MockRepository
                                            .GenerateMock<ITrackImageResizer>();

                                        var converterFactory = MockRepository
                                            .GenerateMock<IHarmonicKeyConverterFactory>();
                                        converterFactory.Stub(f => f.GetAllConverters())
                                                        .Return(new IValueConverter[0]);

                                        var filenameParser = MockRepository.GenerateMock<IFilenameParser>();

                                        filename = "DummyPlaylist.m3u";
                                        reader = new M3uReader(
                                            new TrackLoader(new Id3Reader(),
                                                            new AiffId3Reader(),
                                                            new AacReader(),
                                                            cleanupFactory,
                                                            resizer,
                                                            converterFactory,
                                                            filenameParser));
                                    };

            Because of = () => tracks = reader.Read(filename).Result;

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