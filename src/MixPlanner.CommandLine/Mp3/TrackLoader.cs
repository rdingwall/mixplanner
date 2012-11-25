using System;
using System.IO;
using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.CommandLine.Mp3
{
    public interface ITrackLoader
    {
        Track Load(string filename);
    }

    public class TrackLoader : ITrackLoader
    {
        readonly IId3Reader id3Reader;

        public TrackLoader(IId3Reader id3Reader)
        {
            if (id3Reader == null) throw new ArgumentNullException("id3Reader");
            this.id3Reader = id3Reader;
        }

        public Track Load(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            Track track;
            if (id3Reader.TryRead(filename, out track))
                return track;

            return LoadTrackWithoutId3Tags(filename);
        }

        static Track LoadTrackWithoutId3Tags(string filename)
        {
            var displayName = Path.GetFileNameWithoutExtension(filename);

            return new Track(displayName, Key.Unknown, filename);
        }
    }
}