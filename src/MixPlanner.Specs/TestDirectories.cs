﻿using System.IO;

namespace MixPlanner.Specs
{
    public class TestDirectories
    {
        static readonly TestDirectory LibraryDirectory = new TestDirectory("TestLibrary");
        static readonly TestDirectory DataDirectory = new TestDirectory("TestData");

        public static string ErrorLogFile
        {
            get
            {
                return Path.Combine(DataDirectory.Path, "Errors.log");
            }
        }

        public static string DebugLogFile
        {
            get
            {
                return Path.Combine(DataDirectory.Path, "Debug.log");
            }
        }

        public static void ClearAll()
        {
            DataDirectory.Recreate();
        }

        public static TestDirectory Library { get { return LibraryDirectory; } }
        public static TestDirectory Data { get { return DataDirectory; } }
    }
}