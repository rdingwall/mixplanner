using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using Newtonsoft.Json;
using log4net;

namespace MixPlanner.Storage
{
    public class JsonFileMixStorage : IMixStorage
    {
        readonly IMixFactory mixFactory;
        readonly ITrackLibrary library;
        static readonly ILog Log = LogManager.GetLogger(typeof (JsonFileMixStorage));

        public JsonFileMixStorage(IMixFactory mixFactory, ITrackLibrary library)
        {
            if (mixFactory == null) throw new ArgumentNullException("mixFactory");
            if (library == null) throw new ArgumentNullException("library");
            this.mixFactory = mixFactory;
            this.library = library;
        }

        public async Task SaveAsync(IMix mix, string filename)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (String.IsNullOrWhiteSpace(filename)) 
                throw new ArgumentException("Invalid filename.", "filename");

            var jsonMix = new JsonMix
                              {
                                  Items = mix.Select(ToJsonItem).ToList()
                              };

            try
            {
                Log.DebugFormat("Saving mix {0} ({1} tracks)...", filename, mix.Count);

                long numberOfBytesWritten;

                using (FileStream file = File.Open(filename, FileMode.Create, FileAccess.Write))
                using (var writer = new StreamWriter(file))
                {
                    string json = await JsonConvert.SerializeObjectAsync(
                        jsonMix, GlobalJsonSettings.Formatting, GlobalJsonSettings.Settings);

                    await writer.WriteAsync(json);

                    numberOfBytesWritten = file.Position;
                }

                Log.DebugFormat("Mix saved successfully ({0} bytes).", numberOfBytesWritten);
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error saving mix {0}.", filename), e);
            }
        }

        public async Task<IMix> OpenAsync(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            try
            {
                using (FileStream file = File.OpenRead(filename))
                using (var reader = new StreamReader(file))
                {
                    string json = await reader.ReadToEndAsync();
                    var jsonMix = await JsonConvert.DeserializeObjectAsync<JsonMix>(json, GlobalJsonSettings.Settings);

                    return FromJsonMix(jsonMix);
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error loading mix {0}.", filename), e);
                throw;
            }
        }

        IMix FromJsonMix(JsonMix jsonMix)
        {
            IEnumerable<Tuple<Track, double>> tracks = jsonMix
                .Items.Select(i => Tuple.Create(GetOrCreateTrack(i), i.PlaybackSpeed));

            return mixFactory.Create(tracks);
        }

        Track GetOrCreateTrack(JsonTrack jsonItem)
        {
            Track track;
            if (library.TryGetById(jsonItem.Id, out track))
                return track;

            Log.WarnFormat("{0} - {1} (#{2}) was not found in local library. Creating new track...",
                           jsonItem.Artist, jsonItem.Title, jsonItem.Id);
            return Track.FromJson(jsonItem);
        }

        static JsonMixItem ToJsonItem(IMixItem item)
        {
            return new JsonMixItem
                       {

                           Id = item.Track.Id,
                           Artist = item.Track.Artist,
                           OriginalBpm = item.Track.OriginalBpm,
                           Duration = item.Track.Duration,
                           Filename = item.Track.Filename,
                           Genre = item.Track.Genre,
                           OriginalKey = item.Track.OriginalKey,
                           Label = item.Track.Label,
                           Title = item.Track.Title,
                           Year = item.Track.Year,
                           PlaybackSpeed = item.PlaybackSpeed.Speed
                       };
        }
    }
}