namespace MixPlanner.Commands
{
    using System;
    using System.Diagnostics;

    public sealed class NavigateUriCommand : CommandBase<Uri>
    {
        protected override void Execute(Uri parameter)
        {
            Process.Start(new ProcessStartInfo(parameter.ToString()));
        }
    }
}