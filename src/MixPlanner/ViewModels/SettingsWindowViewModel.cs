using GalaSoft.MvvmLight;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        HarmonicKeyDisplayMode harmonicKeyDisplayMode;
        bool restrictBpmCompatibility;
        bool stripMixedInKeyPrefixes;

        public HarmonicKeyDisplayMode HarmonicKeyDisplayMode
        {
            get { return harmonicKeyDisplayMode; }
            set
            {
                harmonicKeyDisplayMode = value;
                RaisePropertyChanged(() => HarmonicKeyDisplayMode);
            }
        }

        public bool RestrictBpmCompatibility
        {
            get { return restrictBpmCompatibility; }
            set
            {
                restrictBpmCompatibility = value;
                RaisePropertyChanged(() => RestrictBpmCompatibility);
            }
        }

        public bool StripMixedInKeyPrefixes
        {
            get { return stripMixedInKeyPrefixes; }
            set
            {
                stripMixedInKeyPrefixes = value;
                RaisePropertyChanged(() => StripMixedInKeyPrefixes);
            }
        }
    }
}