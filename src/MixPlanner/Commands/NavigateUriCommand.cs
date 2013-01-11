using System;
using System.Diagnostics;

namespace MixPlanner.Commands
{
    public class NavigateUriCommand : CommandBase<Uri>
    {
        protected override void Execute(Uri parameter)
        {
            Process.Start(new ProcessStartInfo(parameter.ToString()));
        }
    }
}