using System;
using System.IO;
using System.Threading.Tasks;
using MixPlanner.Configuration;
using MixPlanner.Storage;
using Newtonsoft.Json;
using log4net;

namespace MixPlanner.IO.ConfigFiles
{
    public interface IConfigWriter
    {
        Task WriteAsync(Config config, string filename);
    }

    public class ConfigWriter : IConfigWriter
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (ConfigWriter));

        public async Task WriteAsync(Config config, string filename)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (filename == null) throw new ArgumentNullException("filename");

            try
            {
                using (FileStream file = File.Open(filename, FileMode.Create, FileAccess.Write))
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
    }
}