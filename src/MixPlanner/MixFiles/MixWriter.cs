using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Storage;
using Newtonsoft.Json;
using log4net;

namespace MixPlanner.MixFiles
{
    public interface IMixWriter
    {
        Task WriteAsync(IMix mix, string filename);
    }

    public class MixWriter : IMixWriter
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (MixWriter));

        public async Task WriteAsync(IMix mix, string filename)
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