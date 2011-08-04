using System;
using System.Collections.Generic;
using System.IO;
using Julana.CommandLine.DomainModel;

namespace Julana.CommandLine.Mp3
{
    public class DirectoryScanner
    {
        readonly IId3Reader id3Reader;
        
        public DirectoryScanner() : this(new Id3Reader()) {}

        public DirectoryScanner(IId3Reader id3Reader)
        {
            if (id3Reader == null) throw new ArgumentNullException("id3Reader");
            this.id3Reader = id3Reader;
        }

        public IEnumerable<Track> GetTracks(string directoryName)
        {
            var files = Directory.GetFiles(directoryName, "*.mp3", SearchOption.AllDirectories);

            IList<Track> tracks = new List<Track>();
            foreach (string filename in files)
            {
                Track track;
                if (id3Reader.TryRead(filename, out track))
                    tracks.Add(track);
            }

            return tracks;
        }
    }
}