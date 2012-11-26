using System;
using Tags.ID3;
using Tags.ID3.ID3v2Frames.TextFrames;
using log4net;
using System.Linq;

namespace MixPlanner.App.Mp3
{
    public interface IId3Reader
    {
        bool TryRead(string filename, out Id3Tag track);
    }

    public class Id3Reader : IId3Reader
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Id3Reader));

        public bool TryRead(string filename, out Id3Tag id3Tag)
        {
            try
            {
                var t = new Id3Tag();

                var id3 = new ID3Info(filename, true);

                // Reference: http://id3.org/id3v2.4.0-frames
                t.InitialKey = GetTextFrameOrDefault(id3, "TKEY") ?? GetUserTextFrame(id3, "Initial key");
                t.Artist = GetTextFrameOrDefault(id3, "TCOM") ?? id3.ID3v1Info.Artist;
                t.Title = GetTextFrameOrDefault(id3, "TIT2") ?? id3.ID3v1Info.Title;
                t.Year = GetId3V1Year(id3) ?? GetTextFrameOrDefault(id3, "TYER");
                t.Genre = GetTextFrameOrDefault(id3, "TCON");
                t.Publisher = GetTextFrameOrDefault(id3, "TPUB") ?? GetUserTextFrame(id3, "Publisher");
                t.Bpm = GetTextFrameOrDefault(id3, "TBPM") ?? GetUserTextFrame(id3, "BPM (beats per minute)");

                id3Tag = t;
                return true;
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error reading ID3 tag from {0}.", filename), e);
                id3Tag = null;
                return false;
            }
        }

        static string GetTextFrameOrDefault(ID3Info id3Info, string frameId)
        {
            if (!id3Info.ID3v2Info.HaveTag)
                return null;

            var value = id3Info.ID3v2Info.GetTextFrame(frameId);

            return String.IsNullOrWhiteSpace(value) ? null : value;
        }

        static string GetUserTextFrame(ID3Info id3Info, string description)
        {
            if (!id3Info.ID3v1Info.HaveTag)
                return null;

            return id3Info.ID3v2Info.UserTextFrames
                          .OfType<UserTextFrame>()
                          .Where(f => f.Description == description)
                          .Select(f => f.Text)
                          .FirstOrDefault();
        }

        static string GetId3V1Year(ID3Info id3Info)
        {
            if (!id3Info.ID3v1Info.HaveTag)
                return null;

            var year = id3Info.ID3v1Info.Year;
            return String.IsNullOrWhiteSpace(year) ? null : year;
        }
    }
}