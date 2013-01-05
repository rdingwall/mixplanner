using System.ComponentModel;

namespace MixPlanner.DomainModel
{
    public class Configuration : INotifyPropertyChanged
    {
        public HarmonicKeyDisplayMode HarmonicKeyDisplayMode { get; set; }
        public bool RestrictBpmCompatibility { get; set; }
        public bool StripMixedInKeyPrefixes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        
    }
}