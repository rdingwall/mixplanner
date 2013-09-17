using System.Collections.Generic;
using MixPlanner.Storage;
using Newtonsoft.Json;

namespace MixPlanner.IO.MixFiles
{
    public class JsonMix
    {
        static readonly string version = FileVersion.CurrentVersion;

        public IList<JsonMixItem> Items { get; set; }

        // For serialization only
        public string Version { get { return version; } }

        [JsonProperty(PropertyName = "_comment")]
        public string Comment { get; set; }
    }
}