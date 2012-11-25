using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.CommandLine.ImportExport
{
    public interface IPlaylistWriter
    {
        void Write(Mix mix, string filename);
    }
}