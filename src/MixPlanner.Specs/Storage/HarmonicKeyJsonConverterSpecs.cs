using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Storage;
using Newtonsoft.Json;

namespace MixPlanner.Specs.Storage
{
    [Subject(typeof(HarmonicKeyJsonConverter))]
    public class HarmonicKeyJsonConverterSpecs
    {
        public class When_checking_what_types_it_can_convert
        {
            It should_be_able_to_convert_harmonic_keys =
                () => new HarmonicKeyJsonConverter().CanConvert(typeof (HarmonicKey)).ShouldBeTrue();
        }

        public class When_writing_json
        {
            Because of = () => json = new HarmonicKeyJsonConverter().WriteJsonString(HarmonicKey.Key6B);

            It should_correctly_serialize_the_key = () => json.ShouldEqual(@"""Bb""");

            static string json;
        }

        public class When_reading_json
        {
            Because of = () => key = new HarmonicKeyJsonConverter().ReadJsonString<HarmonicKey>(@"""Bb""");

            It should_correctly_deserialize_the_key = () => key.ShouldEqual(HarmonicKey.Key6B);

            static HarmonicKey key;
        }
    }
}