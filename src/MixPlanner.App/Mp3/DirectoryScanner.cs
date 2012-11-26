using System;
using System.Collections.Generic;
using System.IO;
using MixPlanner.App.DomainModel;

namespace MixPlanner.App.Mp3
{
    public class DirectoryScanner
    {
        readonly ITrackLoader loader;
        
        public DirectoryScanner(ITrackLoader loader)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            this.loader = loader;
        }

        public IEnumerable<Track> GetTracks(string directoryName)
        {
            var files = Directory.GetFiles(directoryName, "*.mp3", SearchOption.AllDirectories);

            IList<Track> tracks = new List<Track>();
            foreach (var filename in files)
            {
                var track = loader.Load(filename);
                tracks.Add(track);
            }

            return tracks;
        }
    }
}