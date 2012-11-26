using System;
using System.Collections.Generic;
using System.IO;
using MixPlanner.DomainModel;
using MixPlanner.Mp3;
using MoreLinq;

namespace MixPlanner.Storage
{
    public class InMemoryTrackLibrary : ITrackLibrary
    {
        readonly ITrackLoader loader;

        readonly IList<Track> tracks;
        public IEnumerable<Track> Tracks { get { return tracks; } }

        public InMemoryTrackLibrary(ITrackLoader loader)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            this.loader = loader;
            tracks = new List<Track>();
        }

        public void Import(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            var track = loader.Load(filename);
            tracks.Add(track);
        }

        public void Import(IEnumerable<string> filenames)
        {
            if (filenames == null) throw new ArgumentNullException("filenames");

            filenames.ForEach(Import);
        }

        public void ImportDirectory(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException("directoryName");

            var filenames = Directory.GetFiles(directoryName, 
                "*.mp3", SearchOption.AllDirectories);

            Import(filenames);
        }

        public void Remove(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            tracks.Remove(track);
        }
    }
}