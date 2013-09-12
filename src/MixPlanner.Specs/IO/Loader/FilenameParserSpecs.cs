using Machine.Specifications;
using MixPlanner.IO.Loader;

namespace MixPlanner.Specs.IO.Loader
{
    [Subject(typeof(FilenameParser))]
    public class FilenameParserSpecs
    {
         public class When_the_filename_had_a_key_prefix : FixtureBase
         {
             Establish context = 
                 () => Filename = "9A - 3813814_Your Love_Mark Knight Remix.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key = 
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldBeNull();
         }

         public class When_the_filename_had_a_key_suffix : FixtureBase
         {
             Establish context = 
                 () => Filename = "3813814_Your Love_Mark Knight Remix - 9A.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldBeNull();
         }

         public class When_the_filename_had_two_keys_prefix : FixtureBase
         {
             Establish context =
                 () => Filename = "9A or 11B - 3813814_Your Love_Mark Knight Remix.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldBeNull();
         }

         public class When_the_filename_had_two_keys_suffix : FixtureBase
         {
             Establish context =
                 () => Filename = "3813814_Your Love_Mark Knight Remix - 9A or 11B.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldBeNull();
         }

         public class When_the_filename_had_a_key_and_bpm_prefix : FixtureBase
         {
             Establish context = 
                 () => Filename = "9A - 127 - 3813814_Your Love_Mark Knight Remix.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldEqual("127");
         }

         public class When_the_filename_had_a_key_and_bpm_suffix : FixtureBase
         {
             Establish context =
                 () => Filename = "3813814_Your Love_Mark Knight Remix - 9A - 127.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldEqual("127");
         }

         public class When_the_filename_had_two_keys_and_a_bpm_prefix : FixtureBase
         {
             Establish context =
                 () => Filename = "9A or 11A - 127 - 3813814_Your Love_Mark Knight Remix.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldEqual("127");
         }

         public class When_the_filename_had_two_keys_and_a_bpm_suffix : FixtureBase
         {
             Establish context =
                 () => Filename = "3813814_Your Love_Mark Knight Remix - 9A or 11A - 127.wav";

             It should_return_true = () => Result.ShouldBeTrue();

             It should_return_the_correct_key =
                 () => FirstKey.ShouldEqual("9A");

             It should_return_no_bpm =
                 () => Bpm.ShouldEqual("127");
         }

         public class When_the_filename_had_no_keys_or_bpms : FixtureBase
         {
             Establish context = () => Filename = "foo.wav";

             It should_return_false = () => Result.ShouldBeFalse();
         }

        public abstract class FixtureBase
        {
            Because of = () => Result = 
                new FilenameParser().TryParse(Filename, out FirstKey, out Bpm);

            protected static bool Result;
            protected static string FirstKey;
            protected static string Bpm;
            protected static string Filename;
        }
    }
}