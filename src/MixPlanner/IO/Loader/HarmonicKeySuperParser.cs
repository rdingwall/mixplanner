using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.IO.Loader
{
    public interface IHarmonicKeySuperParser
    {
        HarmonicKey ParseHarmonicKey(string key);
    }

    public class HarmonicKeySuperParser : IHarmonicKeySuperParser
    {
        readonly IEnumerable<IValueConverter> notationConverters;

        public HarmonicKeySuperParser(
            IHarmonicKeyConverterFactory converterFactory)
        {
            if (converterFactory == null) throw new ArgumentNullException("converterFactory");
            notationConverters = converterFactory.GetAllConverters();
        }

        public HarmonicKey ParseHarmonicKey(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return HarmonicKey.Unknown;

            HarmonicKey result = notationConverters
                .Select(c => c.ConvertBack(key, typeof(HarmonicKey), null, null))
                .OfType<HarmonicKey>()
                .FirstOrDefault(k => !HarmonicKey.Unknown.Equals(k));

            return result;
        }
    }
}