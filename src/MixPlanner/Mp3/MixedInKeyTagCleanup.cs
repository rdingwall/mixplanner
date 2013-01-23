using System;
using System.Text.RegularExpressions;

namespace MixPlanner.Mp3
{
    public class MixedInKeyTagCleanup : IId3TagCleanup
    {
        readonly Regex keyPrefixPattern;
        readonly Regex keyAndBpmPrefixPattern;
        readonly Regex keySuffixPattern;
        readonly Regex keyAndBpmSuffixPattern;

        public MixedInKeyTagCleanup()
        {
            const RegexOptions options = RegexOptions.Compiled & RegexOptions.IgnoreCase;

            keyPrefixPattern = new Regex(@"^\d{1,2}(A|B)(/\d{1,2}(A|B))?\s-\s", options);
            keyAndBpmPrefixPattern = new Regex(@"^\d{1,2}(A|B)(/\d{1,2}(A|B))?\s-\s\d{1,3}(\.\d)?\s-\s", options);

            keySuffixPattern = new Regex(@"\s-\s\d{1,2}(A|B)(/\d{1,2}(A|B))?$", options);
            keyAndBpmSuffixPattern = new Regex(@"\s-\s\d{1,2}(A|B)(/\d{1,2}(A|B))?\s-\s\d{1,3}(\.\d)?$", options);
        }

         public void Clean(Id3Tag tag)
         {
             if (tag == null) throw new ArgumentNullException("tag");

             tag.Artist = Cleanup(tag.Artist);
             tag.Title = Cleanup(tag.Title);
         }

        string Cleanup(string str)
        {
            str = keyAndBpmPrefixPattern.Replace(str, "");
            str = keyPrefixPattern.Replace(str, "");
            str = keyAndBpmSuffixPattern.Replace(str, "");
            return keySuffixPattern.Replace(str, "");
        }
    }
}