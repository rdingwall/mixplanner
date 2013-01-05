using System;
using System.Windows;
using MixPlanner.ViewModels;

namespace MixPlanner.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(SettingsWindowViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
