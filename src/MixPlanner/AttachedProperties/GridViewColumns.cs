using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MixPlanner.AttachedProperties
{
    public static class GridViewColumns
    {
        public static readonly DependencyProperty ColumnCollectionSourceProperty =
            DependencyProperty.RegisterAttached
                (
                    "ColumnCollectionSource",
                    typeof (IEnumerable),
                    typeof (GridViewColumns),
                    new UIPropertyMetadata(null, ColumnCollectionSourceChanged)
                );

        public static IEnumerable GetColumnCollectionSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(ColumnCollectionSourceProperty);
        }
        public static void SetColumnCollectionSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(ColumnCollectionSourceProperty, value);
        }

        private static void ColumnCollectionSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = obj as GridView;
            if (uiElement == null)
                throw new Exception(String.Format("Object of type '{0}' does not support Columns", obj.GetType()));

            uiElement.Columns.Clear();
            if (e.NewValue == null)
                return;

            var columns = (IEnumerable)e.NewValue;
            foreach (var column in columns.Cast<GridViewColumn>())
                uiElement.Columns.Add(column);
        }
    }
}