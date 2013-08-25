using System;
using System.IO;

namespace MixPlanner.Specs
{
    public class TestDirectory
    {
        readonly string directory;

        public TestDirectory(string directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            this.directory = directory;
        }

        public string Path
        {
            get { return directory; }
        }

        public void Recreate()
        {
            Delete();
            Directory.CreateDirectory(directory);
        }

        public void Touch(string filename)
        {
            CreateFile(filename, "");
        }

        public void Delete()
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, recursive: true);
        }

        public void CreateFile(string filename, string contents)
        {
            var combinedFilename = System.IO.Path.Combine(directory, filename);
            File.WriteAllText(combinedFilename, contents);
        }
    }
}