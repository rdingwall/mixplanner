using MixPlanner.DomainModel;
using Newtonsoft.Json;

namespace MixPlanner.Storage
{
    public class JsonTrack
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "key")]
        [JsonConverter(typeof(HarmonicKeyJsonConverter))]
        public HarmonicKey Key { get; set; }

        [JsonProperty(PropertyName = "bpm")]
        public double Bpm { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }
    }
}