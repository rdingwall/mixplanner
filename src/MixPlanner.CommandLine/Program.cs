using System;
using System.Linq;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.Mp3;

namespace MixPlanner.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var tracks = new DirectoryScanner().GetTracks(Environment.CurrentDirectory);

            var set = new Set(tracks, new NextTrackAdvisor());

            for (;;)
            {
                WriteLine(ConsoleColor.White, "Suggested next tracks (type 0..10 to play):");

                int i = 0;
                var suggestions = set.NextTrackSuggestions().Take(10).ToDictionary(k => i++, v => v);
                foreach (var track in suggestions)
                {
                    WriteLine(ConsoleColor.Gray, "{0}. {1}", track.Key, track.Value.Key);
                }

                var input = Console.ReadLine();
                if (input.Trim() == String.Empty)
                {
                    WriteLine(ConsoleColor.Cyan, "All done. Here's your set list: ");
                    foreach (var track in set.TrackList)
                    {
                        WriteLine(ConsoleColor.Gray, "{0}", track);
                    }
                    Console.ReadLine();
                    return;
                }
                var index = Int32.Parse(input);

                var trackToPlay = suggestions[index].Key;

                WriteLine(ConsoleColor.Green, "Playing {0}!", trackToPlay);

                set.Play(trackToPlay);
            }
        }

        static void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

    }
}
