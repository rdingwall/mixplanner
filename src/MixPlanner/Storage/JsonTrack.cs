using MixPlanner.DomainModel;
using Newtonsoft.Json;

namespace MixPlanner.Storage
{
    public class JsonTrack
    {
        public string Id { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(HarmonicKeyJsonConverter))]
        public HarmonicKey Key { get; set; }

        public double Bpm { get; set; }

        public string Filename { get; set; }
    }
}