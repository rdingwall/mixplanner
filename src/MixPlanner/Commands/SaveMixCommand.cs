using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class SaveMixCommand : AsyncCommandBase<IMix>
    {
        protected override Task DoExecute(IMix parameter)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SaveMixAsCommand : AsyncCommandBase<IMix>
    {
        protected override Task DoExecute(IMix parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}