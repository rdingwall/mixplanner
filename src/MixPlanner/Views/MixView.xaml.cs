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
    /// Interaction logic for MixView.xaml
    /// </summary>
    public partial class MixView : UserControl
    {
        public MixView()
        {
            InitializeComponent();
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Bit dirty, toggle 'IsSelected' flag for virtualized list
            // See http://stackoverflow.com/a/9897347/91551
            var removed = e.RemovedItems.OfType<MixItemViewModel>();
            var added = e.AddedItems.OfType<MixItemViewModel>();

            removed.ForEach(i => i.IsSelected = false);
            added.ForEach(i => i.IsSelected = true);

            var viewModel = (MixViewModel)DataContext;
            viewModel.OnSelectionChanged();
        }
    }
}
