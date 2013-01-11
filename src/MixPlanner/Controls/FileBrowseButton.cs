using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace MixPlanner.Controls
{
    public class FileBrowseButton : Button
    {
        public FileBrowseButton()
        {
            Click += OnClick;
        }

        static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof (string),
            typeof(FileBrowseButton),
            new PropertyMetadata("All files (.*)|*.*"));

        static readonly DependencyProperty FileNamesProperty =
            DependencyProperty.Register("FileNames",
                                        typeof (string[]),
                                        typeof (FileBrowseButton));

        static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName",
                                        typeof (string),
                                        typeof (FileBrowseButton));

        static readonly DependencyProperty MultiSelectProperty =
            DependencyProperty.Register("MutliSelect",
                                        typeof(bool),
                                        typeof(FileBrowseButton),
                                        new PropertyMetadata(false));

        static readonly DependencyProperty InitialDirectoryProperty =
            DependencyProperty.Register("InitialDirectory",
                                        typeof(string),
                                        typeof(FileBrowseButton));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public string[] FileNames
        {
            get { return (string[])GetValue(FileNamesProperty); }
            set { SetValue(FileNamesProperty, value); }
        }

        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        public string InitialDirectory
        {
            get { return (string)GetValue(InitialDirectoryProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        void OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              Filter = Filter,
                              Multiselect = MultiSelect,
                              InitialDirectory = InitialDirectory
                          };

            var result = dlg.ShowDialog();

            if (result == true)
            {
                FileNames = dlg.FileNames;
                FileName = dlg.FileName;
                return;
            }

            e.Handled = true;
        }
    }
}