using System;
using System.IO;
using System.Linq;
using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.CommandLine.ImportExport
{
    public class M3uWriter : IPlaylistWriter
    {
        public void Write(Mix mix, string filename)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (filename == null) throw new ArgumentNullException("filename");

            File.WriteAllLines(filename, mix.Select(t => t.File.FullName));
        }
    }
}