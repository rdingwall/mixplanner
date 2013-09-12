using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using Newtonsoft.Json;
using log4net;

namespace MixPlanner.Storage
{
    public interface IMixReader
    {
        Task<IMix> ReadAsync(string filename);
    }

    public class MixReader : IMixReader
    {
        readonly IMixFactory mixFactory;
        readonly ITrackLibrary library;
        static readonly ILog Log = LogManager.GetLogger(typeof (MixReader));

        public MixReader(IMixFactory mixFactory, ITrackLibrary library)
        {
            if (mixFactory == null) throw new ArgumentNullException("mixFactory");
            if (library == null) throw new ArgumentNullException("library");
            this.mixFactory = mixFactory;
            this.library = library;
        }

        public async Task<IMix> ReadAsync(string filename)
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
                .Items.Select(i => Tuple.Create<Track, double>(GetOrCreateTrack(i), i.PlaybackSpeed));

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
    }
}