using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.Mp3;

namespace MixPlanner.ImportExport
{
    public class M3uReader
    {
        readonly ITrackLoader loader;

        public M3uReader(ITrackLoader loader)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            this.loader = loader;
        }

        public IEnumerable<Track> Read(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            var lines = File.ReadAllLines(filename);

            return lines.Select(l => loader.Load(l));
        }
    }
}