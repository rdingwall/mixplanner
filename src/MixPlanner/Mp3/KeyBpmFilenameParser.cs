using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MixPlanner.DomainModel;
using log4net;

namespace MixPlanner.Mp3
{
    public interface IKeyBpmFilenameParser
    {
        bool TryParse(string filename, out HarmonicKey firstKey, out double bpm);
    }

    public class KeyBpmFilenameParser : IKeyBpmFilenameParser
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (KeyBpmFilenameParser));
        static readonly Regex[] Expressions;

        static KeyBpmFilenameParser()
        {
            const RegexOptions options = RegexOptions.ExplicitCapture &
                                         RegexOptions.Compiled &
                                         RegexOptions.IgnoreCase;

            var keyPrefix = new Regex(@"^(?<key>\d{1,2}(?:A|B))(\sor\s\d{1,2}(A|B))?\s-\s", options);
            var keyAndBpmPrefix = new Regex(@"^(?<key>\d{1,2}(A|B))(\sor\s\d{1,2}(A|B))?\s-\s(?<bpm>\d{1,3}(\.\d)?)\s-\s", options);

            var keySuffix = new Regex(@"\s-\s(?<key>\d{1,2}(A|B))(\sor\s\d{1,2}(A|B))?$", options);
            var keyAndBpmSuffix = new Regex(@"\s-\s(?<key>\d{1,2}(A|B))(\sor\s\d{1,2}(A|B))?\s-\s(?<bpm>\d{1,3}(\.\d)?)$", options);

            Expressions = new[]
                              {
                                  // Most to least specific order
                                  keyAndBpmPrefix,
                                  keyAndBpmSuffix,
                                  keyPrefix,
                                  keySuffix
                              };
        }

        public bool TryParse(string filename, out HarmonicKey firstKey, out double bpm)
        {
            firstKey = null;
            bpm = Double.NaN;

            if (filename == null)
                return false;

            var filenameNoExt = Path.GetFileNameWithoutExtension(filename);

            foreach (Regex expression in Expressions)
            {
                var match = expression.Match(filenameNoExt);
                if (!match.Success)
                    continue;

                try
                {
                    var result = true;
                    if (match.Groups["key"].Success)
                    {
                        var keyStr = match.Groups["key"].Value;
                        result &= HarmonicKey.TryParse(keyStr, out firstKey);
                    }

                    if (match.Groups["bpm"].Success)
                    {
                        var bpmStr = match.Groups["bpm"].Value;
                        result &= Double.TryParse(bpmStr, out bpm);
                    }

                    return result;
                }
                catch (Exception e)
                {
                    Log.WarnFormat("Error when parsing {0} using expression {1}",
                        filenameNoExt, expression);
                    Log.Warn(e);
                }
            }

            return false;
        }

        
    }
}