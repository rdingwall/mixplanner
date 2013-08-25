using System;
using MixPlanner.DomainModel;
using Newtonsoft.Json;

namespace MixPlanner.Storage
{
    public abstract class JsonTrack
    {
        public string Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }

        [JsonConverter(typeof(HarmonicKeyJsonConverter))]
        public HarmonicKey OriginalKey { get; set; }

        public double OriginalBpm { get; set; }
        public string Filename { get; set; }
        public TimeSpan Duration { get; set; }
        public string Label { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
    }
}