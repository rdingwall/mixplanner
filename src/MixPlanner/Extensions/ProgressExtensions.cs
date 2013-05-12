// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class ProgressExtensions
    {
        public static void ReportFormat(this IProgress<string> progress, 
            string format, params object[] args)
        {
            progress.Report(String.Format(format, args));
        }
    }
}