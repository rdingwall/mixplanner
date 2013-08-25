using System.Collections.Generic;

namespace MixPlanner.Storage
{
    public class JsonMix
    {
        static readonly string version = FileVersion.CurrentVersion;

        public IList<JsonMixItem> Items { get; set; }

        // For serialization only
        public string Version { get { return version; } }
    }

    public class JsonMixItem : JsonTrack
    {
        public double PlaybackSpeed { get; set; }
    }
}