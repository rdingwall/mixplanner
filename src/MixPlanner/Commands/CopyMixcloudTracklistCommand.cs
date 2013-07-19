using System;
using System.Text;
using System.Windows;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    /// <summary>
    /// Copies the current mix to the clipboard, as a MixCloud pre-written
    /// track list (unicode text).
    /// </summary>
    public class CopyMixcloudTracklistCommand : CommandBase<IMix>
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

        static string FormatMixcloudPreWrittenTracklist(IMix parameter)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < parameter.Count; i++)
            {
                Track track = parameter[i].Track;

                string artist = ToIdIfBlank(track.Artist);
                string title = ToIdIfBlank(track.Title);

                sb.AppendFormat("{0}. {1} - {2}", i,
                                artist, title);

                if (!String.IsNullOrWhiteSpace(track.Label))
                    sb.AppendFormat(" [{0}]", track.Label);

                sb.AppendLine();
            }

            return sb.ToString();
        }

        static string ToIdIfBlank(string str)
        {
            return String.IsNullOrWhiteSpace(str) ? "ID" : str;
        }
    }
}