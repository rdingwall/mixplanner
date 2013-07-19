using System;

namespace MixPlanner.Loader
{
    public class Tag
    {
        public Tag()
        {
            Artist = "";
            Bpm = "";
            Genre = "";
            InitialKey = "";
            Publisher = "";
            Title = "";
            Year = "";
        }

        public string Artist { get; set; }
        public string Bpm { get; set; }
        public string Genre { get; set; }
        public string InitialKey { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public byte[] ImageData { get; set; }
        public TimeSpan Duration { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Artist, Title);
        }
    }
}