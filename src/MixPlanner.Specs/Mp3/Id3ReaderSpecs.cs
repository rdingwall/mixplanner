using Machine.Specifications;
using MixPlanner.Mp3;

namespace MixPlanner.Specs.Mp3
{
    [Subject(typeof(Id3Reader))]
    public class Id3ReaderSpecs
    {
        public class when_reading_tags_from_a_track_tagged_by_audacity : FixtureBase
        {
            Because of = () => Result = Reader.TryRead(@"audacity.mp3", out Id3Tag);

            It should_get_the_correct_key = 
                () => Id3Tag.InitialKey.ShouldEqual("7A");

            It should_get_the_correct_artist = 
                () => Id3Tag.Artist.ShouldEqual("Hardwell");

            It should_get_the_correct_title = 
                () => Id3Tag.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm = 
                () => Id3Tag.Bpm.ShouldEqual("128");

            It should_get_the_correct_publisher = 
                () => Id3Tag.Publisher.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year = 
                () => Id3Tag.Year.ShouldEqual("2012");

            It should_get_the_correct_genre = 
                () => Id3Tag.Genre.ShouldEqual("Progressive House");
        }

        public class when_reading_tags_from_a_track_tagged_by_mixed_in_key_4 : FixtureBase
        {
            Because of = () => Result = Reader.TryRead(@"mixed_in_key_4.mp3", out Id3Tag);

            It should_get_the_correct_key =
                () => Id3Tag.InitialKey.ShouldEqual("7A");

            It should_get_the_correct_artist =
                () => Id3Tag.Artist.ShouldEqual("7A - 128 - Hardwell");

            It should_get_the_correct_title =
                () => Id3Tag.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm =
                () => Id3Tag.Bpm.ShouldEqual("128");

            It should_get_the_correct_publisher =
                () => Id3Tag.Publisher.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year =
                () => Id3Tag.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Id3Tag.Genre.ShouldEqual("Progressive House");
        }
        
        public class when_reading_tags_from_a_full_track_tagged_by_mixed_in_key_4 : FixtureBase
        {
            Because of = () => Result = Reader.TryRead(@"7A - 128 - 3505135_Three Triangles_Original Club Mix.mp3", out Id3Tag);

            It should_get_the_correct_key =
                () => Id3Tag.InitialKey.ShouldEqual("7A");

            It should_get_the_correct_artist =
                () => Id3Tag.Artist.ShouldEqual("7A - 128 - Hardwell");

            It should_get_the_correct_title =
                () => Id3Tag.Title.ShouldEqual("Three Triangles (Original Club Mix)");

            It should_get_the_correct_bpm =
                () => Id3Tag.Bpm.ShouldEqual("128");

            It should_get_the_correct_publisher =
                () => Id3Tag.Publisher.ShouldEqual("Toolroom Records");

            It should_get_the_correct_year =
                () => Id3Tag.Year.ShouldEqual("2012");

            It should_get_the_correct_genre =
                () => Id3Tag.Genre.ShouldEqual("Progressive House");

            It should_load_an_image =
                () => Id3Tag.ImageData.ShouldNotBeNull();
        }

        public class when_reading_tags_from_a_track_with_only_id3v1_tag : FixtureBase
        {
            Because of = () => Result = Reader.TryRead(@"id3v1_only.mp3", out Id3Tag);

            It should_get_the_correct_key =
                () => Id3Tag.InitialKey.ShouldBeNull();

            It should_get_the_correct_artist =
                () => Id3Tag.Artist.ShouldEqual("Aphex Twin");

            It should_get_the_correct_title =
                () => Id3Tag.Title.ShouldEqual("Bit");

            It should_get_the_correct_bpm =
                () => Id3Tag.Bpm.ShouldBeNull();

            It should_get_the_correct_publisher =
                () => Id3Tag.Publisher.ShouldBeNull();

            It should_get_the_correct_year =
                () => Id3Tag.Year.ShouldEqual("1995");

            It should_get_the_correct_genre =
                () => Id3Tag.Genre.ShouldEqual("Electronic");
        }

        public class when_reading_tags_from_a_track_with_no_initial_key_or_bpm : FixtureBase
        {
            Because of = () => Result = Reader.TryRead(@"id3v2_no_key_or_bpm.mp3", out Id3Tag);

            It should_get_the_correct_key =
                () => Id3Tag.InitialKey.ShouldBeNull();

            It should_get_the_correct_artist =
                () => Id3Tag.Artist.ShouldEqual("Method Man");

            It should_get_the_correct_title =
                () => Id3Tag.Title.ShouldEqual("Say (Call Out)");

            It should_get_the_correct_bpm =
                () => Id3Tag.Bpm.ShouldBeNull();

            It should_get_the_correct_publisher =
                () => Id3Tag.Publisher.ShouldEqual("Def Jam Records");

            It should_get_the_correct_year =
                () => Id3Tag.Year.ShouldEqual("2006");

            It should_get_the_correct_genre =
                () => Id3Tag.Genre.ShouldEqual("Rap");
        }

        public abstract class FixtureBase
        {
            Establish context = () => Reader = new Id3Reader();

            It should_return_true = () => Result.ShouldBeTrue();

            protected static Id3Reader Reader;
            protected static Id3Tag Id3Tag;
            protected static bool Result;
        }
    }
}