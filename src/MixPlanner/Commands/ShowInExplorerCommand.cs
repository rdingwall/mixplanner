using System;
using System.Diagnostics;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class ShowInExplorerCommand : CommandBase<Track>
    {
        protected override bool CanExecute(Track parameter)
        {
            return parameter != null && parameter.HasFilename;
        }

        protected override void Execute(Track parameter)
        {
            var selectedFile = parameter.File.FullName;

            var explorerProcess =
                new Process
                    {
                        StartInfo =
                            {
                                FileName = "explorer.exe",
                                Arguments = String.Format("/select,\"{0}\"", selectedFile)
                            }
                    };

            explorerProcess.Start();
        }
    }
}