using System;
using System.IO;
using System.Threading.Tasks;
using MixPlanner.Configuration;
using Newtonsoft.Json;
using log4net;

namespace MixPlanner.IO.ConfigFiles
{
    public interface IConfigReader
    {
        Task<Config> ReadAsync(string filename);
    }

    public class ConfigReader : IConfigReader
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (ConfigReader));

        public async Task<Config> ReadAsync(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

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