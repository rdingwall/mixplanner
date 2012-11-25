using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.Mp3;

namespace MixPlanner.Specs.Mp3
{
    [Subject(typeof(Id3Reader))]
    public class Id3ReaderSpecifications
    {
        [Ignore("Requires local MP3 file")]
        public class when_reading_the_initial_key_from_an_mp3_file
        {
            Establish context =
                () =>
                    {
                        reader = new Id3Reader();
                    };

            Because of =
                () => 
                reader.TryRead(
                    @"C:\Users\Richard\Desktop\CDJs\10A - 133.9 - 2005934_All We Have Is Now_Original Mix.mp3", out track);

            It should_parse_the_track_without_error = () => track.ShouldNotBeNull();

            It should_get_the_correct_key = () => track.Key.ShouldEqual(Key.Key10A);

            It should_get_the_correct_name = () => track.DisplayName.ShouldEqual("10A - 133.9 - Super8, Tab, Betsie Larkin - All We Have Is Now - Original Mix");

            static Id3Reader reader;
            static Track track;
        }
    }
}