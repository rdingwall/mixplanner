using System;
using System.IO;

namespace MixPlanner.Storage
{
    public static class MixPlannerPaths
    {
        public static string DataDirectory
        {
            get
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, "MixPlanner");
            }
        }

        public static string LibraryDirectory
        {
            get
            {
                return Path.Combine(DataDirectory, "Library");
            }
        }
    }
}