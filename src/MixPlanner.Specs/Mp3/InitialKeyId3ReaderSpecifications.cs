using Machine.Specifications;
using MixPlanner.Mp3;

namespace MixPlanner.Specs.Mp3
{
    [Subject(typeof(Id3Reader))]
    public class Id3ReaderSpecifications
    {
        public class when_reading_id3_tags_from_an_mp3_file_from_audacity
        {
            Establish context = () => reader = new Id3Reader();

            Because of = () => result = reader.TryRead(@"audacity.mp3", out id3Tag);

            It should_return_true = () => result.ShouldBeTrue();

            It should_get_the_correct_key = 
                () => id3Tag.InitialKey.ShouldEqual("7A");

            It should_get_the_correct_artist = 
                () => id3Tag.Artist.ShouldEqual("Hardwell");

            It should_get_the_correct_title = 
                () => id3Tag.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm = 
                () => id3Tag.Bpm.ShouldEqual("128");

            It should_get_the_correct_publisher = 
                () => id3Tag.Publisher.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year = 
                () => id3Tag.Year.ShouldEqual("2012");

            It should_get_the_correct_genre = 
                () => id3Tag.Genre.ShouldEqual("Progressive House");

            static Id3Reader reader;
            static Id3Tag id3Tag;
            static bool result;
        }

        public class when_reading_id3_tags_from_an_mp3_file_from_mixed_in_key_4
        {
            Establish context = () => reader = new Id3Reader();

            Because of = () => result = reader.TryRead(@"mixed_in_key_4.mp3", out id3Tag);

            It should_return_true = () => result.ShouldBeTrue();

            It should_get_the_correct_key =
                () => id3Tag.InitialKey.ShouldEqual("7A");

            It should_get_the_correct_artist =
                () => id3Tag.Artist.ShouldEqual("7A - 128 - Hardwell");

            It should_get_the_correct_title =
                () => id3Tag.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm =
                () => id3Tag.Bpm.ShouldEqual("128");

            It should_get_the_correct_publisher =
                () => id3Tag.Publisher.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year =
                () => id3Tag.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => id3Tag.Genre.ShouldEqual("Progressive House");

            static Id3Reader reader;
            static Id3Tag id3Tag;
            static bool result;
        }
    }
}