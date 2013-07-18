using System;
using System.IO;
using System.Threading.Tasks;
using MixPlanner.Configuration;
using Newtonsoft.Json;
using log4net;

namespace MixPlanner.Storage
{
    public class JsonFileConfigStorage : IConfigStorage
    {
        readonly string filename;
        static readonly ILog Log = LogManager.GetLogger(typeof (JsonFileConfigStorage));

        public JsonFileConfigStorage()
            : this(MixPlannerPaths.DataDirectory)
        {
        }

        public JsonFileConfigStorage(string directory)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            filename = Path.Combine(directory, "MixPlanner.settings");
        }

        public async Task SaveAsync(Config config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                using (FileStream file = File.OpenWrite(filename))
                using (var writer = new StreamWriter(file))
                {
                    string json = await JsonConvert.SerializeObjectAsync(
                        config, GlobalJsonSettings.Formatting, GlobalJsonSettings.Settings);

                    await writer.WriteAsync(json);
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error saving config {0}.", filename), e);
            }
        }

        public async Task<Config> LoadConfigAsync()
        {
            try
            {
                using (FileStream file = File.OpenRead(filename))
                using (var reader = new StreamReader(file))
                {
                    string json = await reader.ReadToEndAsync();
                    return await JsonConvert.DeserializeObjectAsync<Config>(json);
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error loading config {0}.", filename), e);
                return null;
            }
        }
    }
}