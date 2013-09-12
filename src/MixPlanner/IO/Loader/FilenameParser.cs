using System;
using System.IO;
using System.Text.RegularExpressions;
using log4net;

namespace MixPlanner.IO.Loader
{
    public interface IFilenameParser
    {
        bool TryParse(string filename, out string firstKey, out string bpm);
    }

    public class FilenameParser : IFilenameParser
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (FilenameParser));
        static readonly Regex[] Expressions;

        static FilenameParser()
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

        public bool TryParse(string filename, out string firstKey, out string bpm)
        {
            firstKey = null;
            bpm = null;

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
                    if (match.Groups["key"].Success)
                        firstKey = match.Groups["key"].Value;

                    if (match.Groups["bpm"].Success)
                        bpm = match.Groups["bpm"].Value;

                    return true;
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