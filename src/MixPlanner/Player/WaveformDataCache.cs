using System;
using System.Collections.Generic;

namespace MixPlanner.Player
{
    public interface IWaveformDataCache
    {
        bool TryGet(string filename, out float[] waveformData);
        void Add(string filename, float[] waveformData);
    }

    public class WaveformDataCache : IWaveformDataCache
    {
        private readonly IDictionary<string, float[]> cache;

        public WaveformDataCache()
        {
            cache = new Dictionary<string, float[]>(StringComparer.InvariantCultureIgnoreCase);
        }

        public bool TryGet(string filename, out float[] waveformData)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            return cache.TryGetValue(filename, out waveformData);
        }

        public void Add(string filename, float[] waveformData)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (waveformData == null) throw new ArgumentNullException("waveformData");

            if (cache.ContainsKey(filename))
                cache.Remove(filename);

            cache.Add(filename, waveformData);
        }
    }
}