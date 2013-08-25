namespace MixPlanner.Storage
{
    public class JsonLibraryTrack : JsonTrack
    {
        static readonly string version = FileVersion.CurrentVersion;

        // For serialization only
        public string Version { get { return version; } }
    }
}