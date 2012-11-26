using System.Collections.ObjectModel;
using System.Windows.Input;
using MixPlanner.Commands;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            DropFilesCommand = new DropFilesCommand(this);
            LibraryItems = new ObservableCollection<LibraryItemViewModel>();
            LibraryItems.Add(new LibraryItemViewModel { Artist = "Dummy", Filename = @"C:\foo", Key="10A"});
        }

        public ICommand DropFilesCommand { get; private set; }

        public ObservableCollection<LibraryItemViewModel> LibraryItems { get; private set; } 
    }
}