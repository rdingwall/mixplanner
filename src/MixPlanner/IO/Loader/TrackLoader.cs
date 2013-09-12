using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.IO.Loader
{
    public interface ITrackLoader
    {
        bool IsSupportedFileFormat(string filename);
        Task<Track> LoadAsync(string filename);
    }

    public class TrackLoader : ITrackLoader
    {
        readonly IId3Reader id3Reader;
        readonly IAiffId3Reader aiffId3Reader;
        readonly IAacReader aacReader;
        readonly ITagCleanupFactory cleanupFactory;
        readonly ITrackImageResizer imageResizer;
        readonly IHarmonicKeySuperParser keyParser;
        readonly IFilenameParser filenameParser;
        readonly IFallbackReader fallbackReader;

        public TrackLoader(
            IId3Reader id3Reader, 
            IAiffId3Reader aiffId3Reader,
            IAacReader aacReader,
            ITagCleanupFactory cleanupFactory,
            ITrackImageResizer imageResizer, 
            IHarmonicKeySuperParser keyParser, 
            IFilenameParser filenameParser,
            IFallbackReader fallbackReader)
        {
            if (id3Reader == null) throw new ArgumentNullException("id3Reader");
            if (aiffId3Reader == null) throw new ArgumentNullException("aiffId3Reader");
            if (aacReader == null) throw new ArgumentNullException("aacReader");
            if (cleanupFactory == null) throw new ArgumentNullException("cleanupFactory");
            if (imageResizer == null) throw new ArgumentNullException("imageResizer");
            if (keyParser == null) throw new ArgumentNullException("keyParser");
            if (filenameParser == null) throw new ArgumentNullException("filenameParser");
            if (fallbackReader == null) throw new ArgumentNullException("fallbackReader");
            this.id3Reader = id3Reader;
            this.aiffId3Reader = aiffId3Reader;
            this.aacReader = aacReader;
            this.cleanupFactory = cleanupFactory;
            this.imageResizer = imageResizer;
            this.keyParser = keyParser;
            this.filenameParser = filenameParser;
            this.fallbackReader = fallbackReader;
        }

        public bool IsSupportedFileFormat(string filename)
        {
            return FileNameHelper.IsMp3(filename) ||
                   FileNameHelper.IsWav(filename) ||
                   FileNameHelper.IsAiff(filename) ||
                   FileNameHelper.IsAac(filename);
        }

        public async Task<Track> LoadAsync(string filename)
        {
            return await Task.Run(() => Load(filename));
        }

        Track Load(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            if (!IsSupportedFileFormat(filename))
                throw new UnsupportedFileTypeException(filename);

            Tag tag = null;

            if (FileNameHelper.IsMp3(filename))
                id3Reader.TryRead(filename, out tag);

            else if (FileNameHelper.IsAiff(filename))
                aiffId3Reader.TryRead(filename, out tag);

            else if (FileNameHelper.IsAac(filename))
                aacReader.TryRead(filename, out tag);
                
            if (tag == null)
                tag = fallbackReader.Read(filename);

            return CreateTrackFromTag(filename, tag);
        }

        Track CreateTrackFromTag(string filename, Tag tag)
        {
            foreach (var cleanup in cleanupFactory.GetCleanups())
                cleanup.Clean(tag);

            string filenameKey;
            string filenameBpm;
            filenameParser.TryParse(filename, out filenameKey, out filenameBpm);

            var strKey = StringUtils.Coalesce(tag.InitialKey, filenameKey);
            var strBpm = StringUtils.Coalesce(tag.Bpm, filenameBpm);

            HarmonicKey key = keyParser.ParseHarmonicKey(strKey);

            double bpm;
            if (!Double.TryParse(strBpm, out bpm))
                bpm = double.NaN;

            TrackImageData images = 
                tag.ImageData != null ? imageResizer.Process(tag.ImageData) : null;

            var track = new Track(tag.Artist, tag.Title, key, filename, bpm, tag.Duration)
                            {
                                Label = tag.Publisher ?? "",
                                Genre = tag.Genre ?? "",
                                Year = tag.Year ?? "",
                                Images = images
                            };

            return track;
        }
    }
}