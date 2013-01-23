using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Loader;

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

        public async Task<IEnumerable<Track>> Read(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            var lines = File.ReadAllLines(filename);

            var tracks = new List<Track>();
            foreach (var line in lines)
                tracks.Add(await loader.LoadAsync(line));

            return tracks;
        }
    }
}