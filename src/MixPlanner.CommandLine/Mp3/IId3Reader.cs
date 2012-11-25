using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.CommandLine.Mp3
{
    public interface IId3Reader
    {
        bool TryRead(string filename, out Track track);
    }
}