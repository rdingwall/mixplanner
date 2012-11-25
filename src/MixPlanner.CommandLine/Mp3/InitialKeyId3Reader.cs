using System;
using ID3;
using Julana.CommandLine.DomainModel;

namespace Julana.CommandLine.Mp3
{
    public class InitialKeyId3Reader
    {
        public bool TryGetInitialKey(string filename, out Key key)
        {
            try
            {
                var id3 = new ID3Info(filename, true);
                var raw = id3.ID3v2Info.GetTextFrame("TKEY");
                var trimmed = raw.Substring(0, raw.Length - 1);
                key = new Key(trimmed);
                return true;
            }
            catch (Exception)
            {
                key = null;
                return false;
            }
        }
    }
}