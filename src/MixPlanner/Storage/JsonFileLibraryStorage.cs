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
using log4net;

namespace MixPlanner.Storage
{
    public class JsonFileLibraryStorage : ILibraryStorage
    {
        static readonly ImageFormat imageFormat = ImageFormat.Png;
        
        static readonly ILog Log = LogManager.GetLogger(typeof(JsonFileLibraryStorage));

        readonly ITrackImageResizer imageResizer;
        readonly IStorageFilenameFormatter filenameFormatter;
        readonly string libraryDirectory;

        public JsonFileLibraryStorage(ITrackImageResizer imageResizer)
            : this(imageResizer, directory: MixPlannerPaths.LibraryDirectory)
        {
        }

        public JsonFileLibraryStorage(ITrackImageResizer imageResizer,
            string directory)
        {
            if (imageResizer == null) throw new ArgumentNullException("imageResizer");
            if (directory == null) throw new ArgumentNullException("directory");
            this.imageResizer = imageResizer;
            filenameFormatter = new StorageFilenameFormatter(directory, "png");
            libraryDirectory = directory;
            EnsureDirectoryExists();
        }

        public async Task<IEnumerable<Track>> LoadAllTracksAsync()
        {
            Task<Track>[] tasks =
                Directory.GetFiles(libraryDirectory, filenameFormatter.SearchPattern)
                         .Select(LoadAsync)
                         .ToArray();

            Task.WaitAll(tasks);

            return tasks.Select(t => t.Result).Where(t => t != null);
        }

        async Task<Track> LoadAsync(string filename)
        {
            try
            {
                JsonTrack jsonTrack = await ReadTrackDataAsync(filename);

                if (jsonTrack == null)
                    return null;

                TrackImageData imageData = await ReadImageDataAsync(filename);

                return new Track(id: jsonTrack.Id,
                                 artist: jsonTrack.Artist,
                                 title: jsonTrack.Title,
                                 originalKey: jsonTrack.Key,
                                 originalBpm: jsonTrack.Bpm,
                                 fileName: jsonTrack.Filename,
                                 duration: jsonTrack.Duration)
                           {
                               Images = imageData,
                               Genre = jsonTrack.Genre,
                               Label = jsonTrack.Label,
                               Year = jsonTrack.Year
                           };
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error loading track {0}.", filename), e);
                return null;
            }
        }

        public async Task AddTrackAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            await WriteTrackAsync(track, FileMode.Create).ContinueWith(_ => WriteImage(track));
        }

        public Task RemoveTrackAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");

            return Task.Run(() => TryDelete(filenameFormatter.FormatCoverArtFilename(track)))
                       .ContinueWith(_ => TryDelete(filenameFormatter.FormatTrackFilename(track)));
        }

        void WriteImage(Track track)
        {
            if (!track.HasImages)
                return;

            var filename = filenameFormatter.FormatCoverArtFilename(track);

            using (var stream = new MemoryStream(track.GetFullSizeImageBytes()))
            using (var image = Image.FromStream(stream))
                image.Save(filename, imageFormat);
        }

        async Task WriteTrackAsync(Track track, FileMode fileMode)
        {
            var jsonTrack = new JsonTrack
            {
                Id = track.Id,
                Artist = track.Artist,
                Title = track.Title,
                Key = track.OriginalKey,
                Bpm = track.OriginalBpm,
                Filename = track.Filename,
                Duration = track.Duration,
                Genre = track.Genre,
                Year = track.Year,
                Label = track.Label
            };

            string filename = filenameFormatter.FormatTrackFilename(track);

            await JsonConvert.SerializeObjectAsync(jsonTrack, GlobalJsonSettings.Formatting, GlobalJsonSettings.Settings)
                             .ContinueWith(
                                 async t =>
                                 {
                                     try
                                     {
                                         using (var file = File.Open(filename, fileMode))
                                         using (var writer = new StreamWriter(file))
                                             await writer.WriteAsync(t.Result);
                                     }
                                     catch (Exception e)
                                     {
                                         Log.Error(String.Format("Failed to write {0}.", filename), e);
                                     }
                                 });
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

        public async Task UpdateTrackAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            await WriteTrackAsync(track, FileMode.Truncate);
        }

        static async Task<JsonTrack> ReadTrackDataAsync(string filename)
        {
            using (FileStream file = File.OpenRead(filename))
            using (var reader = new StreamReader(file))
            {
                string json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<JsonTrack>(json);
            }
        }

        async Task<TrackImageData> ReadImageDataAsync(string trackFilename)
        {
            string imageFilename = filenameFormatter.GetCorrespondingImageFilename(trackFilename);

            if (!File.Exists(imageFilename))
                return null;

            try
            {
                using (FileStream file = File.OpenRead(imageFilename))
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    return imageResizer.Process(memoryStream.ToArray());
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error loading covert art {0}.", imageFilename), e);
                return null;
            }
        }

        void EnsureDirectoryExists()
        {
            if (!Directory.Exists(libraryDirectory))
                Directory.CreateDirectory(libraryDirectory);
        }
    }
}