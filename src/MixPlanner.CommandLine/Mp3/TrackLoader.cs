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

            Id3Tag id3Tag;
            if (id3Reader.TryRead(filename, out id3Tag))
            {
                var displayName = String.Format("{0} - {1}", id3Tag.Artist, id3Tag.Title);
                Key key;
                if (!Key.TryParse(id3Tag.InitialKey, out key))
                    key = Key.Unknown;

                return new Track(displayName, key, filename);
            }

            return LoadTrackWithoutId3Tags(filename);
        }

        static Track LoadTrackWithoutId3Tags(string filename)
        {
            var displayName = Path.GetFileNameWithoutExtension(filename);

            return new Track(displayName, Key.Unknown, filename);
        }
    }
}