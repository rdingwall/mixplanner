using System;
using System.IO;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.ImportExport
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