using MixPlanner.App.DomainModel;

namespace MixPlanner.App.ImportExport
{
    public interface IPlaylistWriter
    {
        void Write(Mix mix, string filename);
    }
}