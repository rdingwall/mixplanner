using MixPlanner.DomainModel;

namespace MixPlanner.ImportExport
{
    public interface IPlaylistWriter
    {
        void Write(Mix mix, string filename);
    }
}