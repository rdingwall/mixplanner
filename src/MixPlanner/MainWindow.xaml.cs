using System.Windows;
using MixPlanner.ViewModels;

namespace MixPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        void MainWindow_OnDragEnter(object sender, DragEventArgs e)
        {
            var dropPossible = e.Data != null && ((DataObject)e.Data).ContainsFileDropList();
            if (dropPossible)
                e.Effects = DragDropEffects.Copy;
        }
    }
}
