using Machine.Specifications;
using MixPlanner.Mp3;

namespace MixPlanner.Specs.Mp3
{
    [Subject(typeof(MixedInKeyTagCleanup))]
    public class MixedInKeyPrefixCleanupSpecs
    {
         public class When_cleaning_up_id3_tags_that_have_the_key_before_the_artist_name
         {
             Establish context = () => tag = new Id3Tag {Artist = "10A - Axwell"};

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Axwell");

             static Id3Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_the_key_and_bpm_before_the_artist_name
         {
             Establish context = () => tag = new Id3Tag { Artist = "1A - 125 - Dustin Zahn" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Dustin Zahn");

             static Id3Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_the_key_after_the_artist_name
         {
             Establish context = () => tag = new Id3Tag { Artist = "Axwell - 10A" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Axwell");

             static Id3Tag tag;
         }

         public class When_cleaning_up_id3_tags_that_have_the_key_and_bpm_after_the_artist_name
         {
             Establish context = () => tag = new Id3Tag { Artist = "Axwell - 10A - 126" };

             Because of = () => new MixedInKeyTagCleanup().Clean(tag);

             It should_strip_the_key_prefix = () => tag.Artist.ShouldEqual("Axwell");

             static Id3Tag tag;
         }
    }
}