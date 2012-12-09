using System;
using System.Text.RegularExpressions;

namespace MixPlanner.Mp3
{
    public class MixedInKeyTagCleanup : IId3TagCleanup
    {
        readonly string keyPrefixPattern;
        readonly string keyAndBpmPrefixPattern;
        readonly string keySuffixPattern;
        readonly string keyAndBpmSuffixPattern;

        public MixedInKeyTagCleanup()
        {
            keyPrefixPattern = @"^\d{1,2}(A|B)\s-\s";
            keyAndBpmPrefixPattern = @"^\d{1,2}(A|B)\s-\s\d{1,3}(\.\d)?\s-\s";

            keySuffixPattern = @"\s-\s\d{1,2}(A|B)$";
            keyAndBpmSuffixPattern = @"\s-\s\d{1,2}(A|B)\s-\s\d{1,3}(\.\d)?$";
        }

         public void Clean(Id3Tag tag)
         {
             if (tag == null) throw new ArgumentNullException("tag");

             tag.Artist = Cleanup(tag.Artist);
             tag.Title = Cleanup(tag.Title);
         }

        string Cleanup(string str)
        {
            str = Regex.Replace(str, keyAndBpmPrefixPattern, "",
                                       RegexOptions.Compiled | RegexOptions.IgnoreCase);

            str = Regex.Replace(str, keyPrefixPattern, "",
                                       RegexOptions.Compiled | RegexOptions.IgnoreCase);

            str = Regex.Replace(str, keyAndBpmSuffixPattern, "",
                                       RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return Regex.Replace(str, keySuffixPattern, "",
                                       RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}