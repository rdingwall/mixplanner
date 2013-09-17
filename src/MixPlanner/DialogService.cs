using System;
using Microsoft.Win32;

namespace MixPlanner
{
    public interface IDialogService
    {
        bool TrySaveMix(out string filename);
    }

    public class DialogService : IDialogService
    {
        public bool TrySaveMix(out string filename)
        {
            var dialog = new SaveFileDialog
                             {
                                 FileName = "Untitled",
                                 DefaultExt = ".mx",
                                 Filter = "MixPlanner mix files|*.mx"
                             };

            filename = null;

            if (dialog.ShowDialog() == false)
                return false;

            filename = dialog.FileName;
            return true;
        }
    }
}