namespace MixPlanner.Commands
{
    using System.Diagnostics;
    using MixPlanner.DomainModel;

    public sealed class ShowInExplorerCommand : CommandBase<Track>
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
                                Arguments = string.Format("/select,\"{0}\"", selectedFile)
                            }
                    };

            explorerProcess.Start();
        }
    }
}