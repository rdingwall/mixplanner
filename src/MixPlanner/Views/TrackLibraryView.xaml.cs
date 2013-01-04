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
using MixPlanner.ViewModels;
using MoreLinq;

namespace MixPlanner.Views
{
    /// <summary>
    /// Interaction logic for TrackLibraryView.xaml
    /// </summary>
    public partial class TrackLibraryView : UserControl
    {
        public TrackLibraryView()
        {
            InitializeComponent();
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Bit dirty, toggle 'IsSelected' flag for virtualized list
            // See http://stackoverflow.com/a/9897347/91551
            e.RemovedItems.OfType<TrackLibraryItemViewModel>().ForEach(i => i.IsSelected = false);
            e.AddedItems.OfType<TrackLibraryItemViewModel>().ForEach(i => i.IsSelected = true);
        }
    }
}
