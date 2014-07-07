namespace MixPlanner.Commands
{
    using MixPlanner.ViewModels;

    public sealed class CloseWindowCommand : CommandBase<CloseableViewModelBase>
    {
        protected override void Execute(CloseableViewModelBase parameter)
        {
            parameter.Close = true;
        }
    }
}