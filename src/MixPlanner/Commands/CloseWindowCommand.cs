using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class CloseWindowCommand : CommandBase<CloseableViewModelBase>
    {
        protected override void Execute(CloseableViewModelBase parameter)
        {
            parameter.Close = true;
        }
    }
}