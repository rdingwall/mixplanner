using System;
using System.Windows;
using System.Windows.Input;
using MixPlanner.App.ViewModels;
using MixPlanner.CommandLine.Mp3;

namespace MixPlanner.App.Commands
{
    public class DropFilesCommand : ICommand
    {
        readonly MainWindowViewModel parent;
        readonly ITrackLoader trackLoader;

        public DropFilesCommand(MainWindowViewModel parent)
        {
            this.parent = parent;
            trackLoader = new TrackLoader(new Id3Reader());
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var e = parameter as DragEventArgs;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                var track = trackLoader.Load(file);
                parent.LibraryItems.Add(new LibraryItemViewModel
                                            {
                                                DisplayName = track.DisplayName,
                                                Filename = track.File.FullName,
                                                Key = track.Key.ToString()
                                            });
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}