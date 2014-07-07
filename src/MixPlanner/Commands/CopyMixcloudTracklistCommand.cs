namespace MixPlanner.Commands
{
    using System.Text;
    using System.Windows;
    using MixPlanner.DomainModel;

    /// <summary>
    /// Copies the current mix to the clipboard, as a MixCloud pre-written
    /// track list (unicode text).
    /// </summary>
    public sealed class CopyMixcloudTracklistCommand : CommandBase<IMix>
    {
        protected override bool CanExecute(IMix parameter)
        {
            return !parameter.IsEmpty;
        }

        protected override void Execute(IMix parameter)
        {
            string tracklist = FormatMixcloudPreWrittenTracklist(parameter);
            Clipboard.SetData(DataFormats.UnicodeText, tracklist);
        }

        private static string FormatMixcloudPreWrittenTracklist(IMix parameter)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < parameter.Count; i++)
            {
                Track track = parameter[i].Track;

                string artist = ToIdIfBlank(track.Artist);
                string title = ToIdIfBlank(track.Title);

                sb.AppendFormat("{0}. {1} - {2}", i, artist, title);

                if (!string.IsNullOrWhiteSpace(track.Label))
                {
                    sb.AppendFormat(" [{0}]", track.Label);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string ToIdIfBlank(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "ID" : str;
        }
    }
}