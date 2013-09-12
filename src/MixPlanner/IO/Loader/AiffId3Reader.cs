using TagLib;

namespace MixPlanner.IO.Loader
{
    public interface IAiffId3Reader : IId3Reader
    {
    }

    public class AiffId3Reader : Id3Reader, IAiffId3Reader
    {
        protected override File OpenFile(string filename)
        {
            return File.Create(filename, "audio/x-aiff", ReadStyle.None);
        }
    }
}