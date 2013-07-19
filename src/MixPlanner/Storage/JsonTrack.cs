using System;
using System.Reflection;
using MixPlanner.DomainModel;
using Newtonsoft.Json;

namespace MixPlanner.Storage
{
    public class JsonTrack
    {
        static readonly string version =
            Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string Id { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(HarmonicKeyJsonConverter))]
        public HarmonicKey Key { get; set; }

        public double Bpm { get; set; }

        public string Filename { get; set; }

        public TimeSpan Duration { get; set; }

        // For serialization only
        public string Version { get { return version; } }
    }
}