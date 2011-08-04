using Julana.CommandLine.DomainModel;
using Julana.CommandLine.Mp3;
using Machine.Specifications;

namespace Julana.Specs.Mp3
{
    [Subject(typeof(InitialKeyId3Reader))]
    public class InitialKeyId3ReaderSpecifications
    {
        public class when_reading_the_initial_key_from_an_mp3_file
        {
            Establish context =
                () =>
                    {
                        reader = new InitialKeyId3Reader();
                    };

            Because of =
                () => 
                reader.TryGetInitialKey(
                    @"C:\Users\Richard\Desktop\CDJs\10A - 133.9 - 2005934_All We Have Is Now_Original Mix.mp3", out key);

            It should_get_the_correct_key = () => key.ShouldEqual(Key.Key10A);

            static InitialKeyId3Reader reader;
            static Key key;
        }
    }
}