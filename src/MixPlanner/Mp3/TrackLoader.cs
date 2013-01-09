using System;
using System.IO;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Mp3
{
    public interface ITrackLoader
    {
        Task<Track> LoadAsync(string filename);
    }

    public class TrackLoader : ITrackLoader
    {
        readonly IId3Reader id3Reader;
        readonly IId3TagCleanupFactory cleanupFactory;

        public TrackLoader(IId3Reader id3Reader, IId3TagCleanupFactory cleanupFactory)
        {
            if (id3Reader == null) throw new ArgumentNullException("id3Reader");
            if (cleanupFactory == null) throw new ArgumentNullException("cleanupFactory");
            this.id3Reader = id3Reader;
            this.cleanupFactory = cleanupFactory;
        }

        public async Task<Track> LoadAsync(string filename)
        {
            return await Task.Run(() => Load(filename));
        }

        Track Load(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            Id3Tag id3Tag;
            if (id3Reader.TryRead(filename, out id3Tag))
                return LoadTrackFromId3Tag(filename, id3Tag);

            return LoadTrackWithoutId3Tags(filename);
        }

        Track LoadTrackFromId3Tag(string filename, Id3Tag id3Tag)
        {
            foreach (var cleanup in cleanupFactory.GetCleanups())
                cleanup.Clean(id3Tag);

            HarmonicKey key;
            if (!HarmonicKey.TryParse(id3Tag.InitialKey, out key))
                key = HarmonicKey.Unknown;

            double bpm;
            if (!Double.TryParse(id3Tag.Bpm, out bpm))
                bpm = double.NaN;

            var track = new Track(id3Tag.Artist, id3Tag.Title, key, filename, bpm)
                            {
                                Label = id3Tag.Publisher ?? "",
                                Genre = id3Tag.Genre ?? "",
                                Year = id3Tag.Year ?? ""
                            };

            return track;
        }

        static Track LoadTrackWithoutId3Tags(string filename)
        {
            var displayName = Path.GetFileNameWithoutExtension(filename);

            return new Track("Unknown Artist", displayName, 
                HarmonicKey.Unknown, filename, float.NaN);
        }
    }
}