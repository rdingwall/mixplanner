namespace MixPlanner.App.ViewModels
{
    public class LibraryItemViewModel
    {
        public string Filename { get; set; }

        public string Artist { get; set; }
        public string Title { get; set; }

        public string Key { get; set; }

        public string Genre { get; set; }
        public string Label { get; set; }
        public string Year { get; set; }
        public int? Bpm { get; set; }
    }
}