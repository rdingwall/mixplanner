using Julana.CommandLine.DomainModel;

namespace Julana.CommandLine.Mp3
{
    public interface IId3Reader
    {
        bool TryRead(string filename, out Track track);
    }
}