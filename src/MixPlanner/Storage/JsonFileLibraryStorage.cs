using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MixPlanner.Storage
{
    public class JsonFileLibraryStorage : ILibraryStorage
    {
        const string LibraryDirectory = "Library";
        
        public JsonFileLibraryStorage()
        {
            EnsureDirectoryExists();
        }

        public async Task<IEnumerable<Track>> FetchAllAsync()
        {
            Task<Track>[] tasks = Directory.GetFiles(LibraryDirectory, "*.track")
                     .Select(LoadAsync)
                     .ToArray();

            Task.WaitAll(tasks);

            return tasks.Select(t => t.Result);
        }

        static async Task<Track> LoadAsync(string filename)
        {
            using (var file = File.OpenRead(filename))
            using (var reader = new StreamReader(file))
            {
                string json = await reader.ReadToEndAsync();
                var jsonTrack = JsonConvert.DeserializeObject<JsonTrack>(json);

                HarmonicKey key = HarmonicKey.Unknown;
                HarmonicKey.TryParse(jsonTrack.Key, out key);

                return new Track(id: jsonTrack.Id,
                                 artist: jsonTrack.Artist,
                                 title: jsonTrack.Title,
                                 originalKey: key,
                                 originalBpm: jsonTrack.Bpm,
                                 fileName: jsonTrack.Filename);
            }
        }

        public async Task AddAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            await WriteTrack(track, FileMode.Create);
        }

        static async Task WriteTrack(Track track, FileMode fileMode)
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

            string filename = FormatFilename(track);

            await JsonConvert.SerializeObjectAsync(jsonTrack, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
                             .ContinueWith(
                                 t =>
                                 {
                                     using (var file = File.Open(filename, fileMode))
                                     using (var writer = new StreamWriter(file))
                                         writer.WriteAsync(t.Result);
                                 });
        }

        static string FormatFilename(Track track)
        {
            return Path.Combine(LibraryDirectory, string.Format("{0}.track", track.Id));
        }

        static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(LibraryDirectory))
                Directory.CreateDirectory(LibraryDirectory);
        }

        public async Task RemoveAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            string filename = FormatFilename(track);
            if (File.Exists(filename))
                File.Delete(filename);
        }

        public async Task UpdateAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            await WriteTrack(track, FileMode.Truncate);
        }
    }
}