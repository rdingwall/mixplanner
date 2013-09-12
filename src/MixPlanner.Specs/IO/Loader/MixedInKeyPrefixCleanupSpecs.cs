using Machine.Specifications;
using MixPlanner.IO.Loader;

namespace MixPlanner.Specs.IO.Loader
{
    [Subject(typeof(MixedInKeyTagCleanup))]
    public class MixedInKeyPrefixCleanupSpecs
    {
         public class When_cleaning_up_id3_tags_that_have_the_key_before_the_artist_name
         {
             Establish context = () => tag = new Tag {Artist = "10A - Axwell"};

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Axwell");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_two_keys_before_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "1A/11A - Cosmic Gate" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Cosmic Gate");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_the_key_and_bpm_before_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "1A - 125.8 - Dustin Zahn" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Dustin Zahn");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_two_keys_and_the_bpm_before_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "1A/11A - 132 - Cosmic Gate" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Cosmic Gate");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_the_key_after_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "Axwell - 10A" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Axwell");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_two_keys_after_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "Cosmic Gate - 1A/11A" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Cosmic Gate");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_the_key_and_bpm_after_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "Axwell - 10A - 126.1" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Axwell");

             static Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_two_keys_and_the_bpm_after_the_artist_name
         {
             Establish context = () => tag = new Tag { Artist = "Cosmic Gate - 1A/11A - 132" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Cosmic Gate");

             static Tag tag;
         }
    }
}