using System;
using System.Linq;
using System.Windows.Controls;

namespace MixPlanner.Controls
{
    public class YearOnlyTextBox : TextBox
    {
        // Based on http://dedjo.blogspot.co.uk/2007/11/number-only-textbox.html
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(Char.IsDigit);
            base.OnPreviewTextInput(e);
        }
    }
}