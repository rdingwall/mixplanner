using System;
using System.Windows;
using System.Windows.Input;
using MixPlanner.App.Mp3;
using MixPlanner.App.ViewModels;

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
                                                Artist = track.Artist,
                                                Title = track.Title,
                                                Genre = track.Genre,
                                                Bpm = track.Bpm,
                                                Year = track.Year,
                                                Label = track.Label,
                                                Filename = track.File.FullName,
                                                Key = track.Key.ToString()
                                            });
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}