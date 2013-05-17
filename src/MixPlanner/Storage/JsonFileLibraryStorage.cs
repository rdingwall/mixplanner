using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Loader;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using log4net;

namespace MixPlanner.Storage
{
    public class JsonFileLibraryStorage : ILibraryStorage
    {
        const Formatting jsonFormatting = Formatting.Indented;
        static readonly ImageFormat imageFormat = ImageFormat.Png;
        static readonly JsonSerializerSettings jsonSettings
            = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        static readonly ILog Log = LogManager.GetLogger(typeof(JsonFileLibraryStorage));

        readonly ITrackImageResizer imageResizer;
        readonly IStorageFilenameFormatter filenameFormatter;
        readonly string libraryDirectory;

        public JsonFileLibraryStorage(ITrackImageResizer imageResizer)
            : this(imageResizer, directory: "Library")
        {
        }

        public JsonFileLibraryStorage(ITrackImageResizer imageResizer,
            string directory)
        {
            if (imageResizer == null) throw new ArgumentNullException("imageResizer");
            if (filenameFormatter == null) throw new ArgumentNullException("filenameFormatter");
            if (directory == null) throw new ArgumentNullException("directory");
            this.imageResizer = imageResizer;
            filenameFormatter = new StorageFilenameFormatter(directory, "png");
            libraryDirectory = directory;
            EnsureDirectoryExists();
        }

        public async Task<IEnumerable<Track>> FetchAllAsync()
        {
            Task<Track>[] tasks = Directory.GetFiles(libraryDirectory, "*.track")
                     .Select(LoadAsync)
                     .ToArray();

            Task.WaitAll(tasks);

            return tasks.Select(t => t.Result).Where(t => t != null);
        }

        async Task<Track> LoadAsync(string filename)
        {
            try
            {
                JsonTrack jsonTrack;
                using (var file = File.OpenRead(filename))
                using (var reader = new StreamReader(file))
                {
                    string json = await reader.ReadToEndAsync();
                    jsonTrack = JsonConvert.DeserializeObject<JsonTrack>(json);
                }

                string imageFilename = filenameFormatter.GetCorrespondingImageFilename(filename);

                TrackImageData imageData = null;
                if (File.Exists(imageFilename))
                {
                    using (var imageFile = File.OpenRead(imageFilename))
                    using (var image = Image.FromStream(imageFile))
                    using (var bitmapStream = new MemoryStream())
                    {
                        image.Save(bitmapStream, ImageFormat.Bmp);
                        imageData = imageResizer.Process(bitmapStream.ToArray());
                    }
                }

                HarmonicKey key = HarmonicKey.Unknown;
                HarmonicKey.TryParse(jsonTrack.Key, out key);

                return new Track(id: jsonTrack.Id,
                                 artist: jsonTrack.Artist,
                                 title: jsonTrack.Title,
                                 originalKey: key,
                                 originalBpm: jsonTrack.Bpm,
                                 fileName: jsonTrack.Filename) { Images = imageData };
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error loading {0}.", filename), e);
                return null;
            }
        }

        public async Task AddAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            await WriteTrack(track, FileMode.Create).ContinueWith(_ => WriteImage(track));
        }

        void WriteImage(Track track)
        {
            if (track.Images == null)
                return;

            var filename = filenameFormatter.FormatCoverArtFilename(track);

            using (var stream = new MemoryStream(track.Images.FullSize.Data))
            using (var image = Image.FromStream(stream))
                image.Save(filename, imageFormat);
        }

        async Task WriteTrack(Track track, FileMode fileMode)
        {
            var jsonTrack = new JsonTrack
            {
                Id = track.Id,
                Artist = track.Artist,
                Title = track.Title,
                Key = track.OriginalKey.ToString(),
                Bpm = track.OriginalBpm,
                Filename = track.Filename
            };

            string filename = filenameFormatter.FormatTrackFilename(track);

            await JsonConvert.SerializeObjectAsync(jsonTrack, jsonFormatting, jsonSettings)
                             .ContinueWith(
                                 t =>
                                 {
                                     try
                                     {
                                         using (var file = File.Open(filename, fileMode))
                                         using (var writer = new StreamWriter(file))
                                             writer.WriteAsync(t.Result);
                                     }
                                     catch (Exception e)
                                     {
                                         Log.Error(String.Format("Failed to write {0}.", filename), e);
                                     }
                                 });
        }

        void EnsureDirectoryExists()
        {
            if (!Directory.Exists(libraryDirectory))
                Directory.CreateDirectory(libraryDirectory);
        }

        public async Task RemoveAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");

            TryDelete(filenameFormatter.FormatCoverArtFilename(track));
            TryDelete(filenameFormatter.FormatTrackFilename(track));
        }

        static void TryDelete(string filename)
        {
            try
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Failed to delete {0}.", filename), e);
            }
        }

        public async Task UpdateAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            await WriteTrack(track, FileMode.Truncate);
        }
    }
}