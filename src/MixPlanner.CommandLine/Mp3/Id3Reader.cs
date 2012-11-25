using System;
using ID3;
using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.CommandLine.Mp3
{
    public class Id3Reader : IId3Reader
    {
        public bool TryRead(string filename, out Track track)
        {
            try
            {
                var id3 = new ID3Info(filename, true);
                var key = GetKey(id3);

                var artist = id3.ID3v2Info.GetTextFrame("TPE1");
                var title = id3.ID3v2Info.GetTextFrame("TIT2");

                track = new Track(String.Format("{0} - {1}", TrimBogusChar(artist), TrimBogusChar(title)), key);
                return true;
            }
            catch (Exception)
            {
                track = null;
                return false;
            }
        }

        static Key GetKey(ID3Info id3)
        {
            var tkey = id3.ID3v2Info.GetTextFrame("TKEY");
            return new Key(TrimBogusChar(tkey));
        }

        static string TrimBogusChar(string raw)
        {
            return raw.Substring(0, raw.Length - 1);
        }
    }
}