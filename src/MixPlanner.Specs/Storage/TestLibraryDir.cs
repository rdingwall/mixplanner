using System.IO;

namespace MixPlanner.Specs.Storage
{
    public static class TestLibraryDir
    {
        public const string DirectoryName = "TestLibrary";

        public static void Recreate()
        {
            Delete();
            Directory.CreateDirectory(DirectoryName);
        }

        public static void Touch(string filename)
        {
            CreateFile(filename, "");
        }

        public static void Delete()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, recursive: true);
        }

        public static void CreateFile(string filename, string contents)
        {
            var combinedFilename = Path.Combine(DirectoryName, filename);
            File.WriteAllText(combinedFilename, contents);
        }
    }
}