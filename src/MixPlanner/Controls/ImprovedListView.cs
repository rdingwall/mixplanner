using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixPlanner.Controls
{
    public class ImprovedListView : ListView
    {
        public ImprovedListView()
        {
            PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUpX;
        }

        static readonly Key[] SelectionModifierKeys = new[]
                                                          {
                                                              Key.LeftShift,
                                                              Key.RightShift,
                                                              Key.LeftCtrl,
                                                              Key.RightCtrl
                                                          };

        void OnPreviewMouseLeftButtonUpX(object sender, MouseButtonEventArgs e)
        {
            //search the object hierarchy for a datagrid row
            var source = (DependencyObject)e.OriginalSource;

            if (SelectionModifierKeys.Any(Keyboard.IsKeyDown))
                return;

            var row = source.TryFindParent<DataGridRow>();
            if (row == null)
                return;

            if (SelectedItems.Count > 1)
                SelectedIndex = row.GetIndex();

            //[insert great code here...]

            e.Handled = true;
        }
    }
}