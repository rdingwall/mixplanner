using System.Reflection;

namespace MixPlanner.Storage
{
    public static class FileVersion
    {
        static readonly string currentVersion = 
            Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string CurrentVersion { get { return currentVersion; } }
    }
}