using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MixPlanner.App.ViewModels;

namespace MixPlanner.App
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
