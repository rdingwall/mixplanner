using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MixPlanner.ViewModels;

namespace MixPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
