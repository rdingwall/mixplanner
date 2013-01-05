using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        HarmonicKeyDisplayMode harmonicKeyDisplayMode;
        bool restrictBpmCompatibility;
        bool stripMixedInKeyPrefixes;
        bool close;

        public ICommand SaveCommand { get; private set; }

        public SettingsWindowViewModel(
            IDispatcherMessenger messenger,
            SaveSettingsCommand saveCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (saveCommand == null) throw new ArgumentNullException("saveCommand");
            SaveCommand = saveCommand;

            messenger.Register<ConfigurationSavedEvent>(this, _ => Close = true);
        }

        public void Initialize(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            HarmonicKeyDisplayMode = configuration.HarmonicKeyDisplayMode;
            RestrictBpmCompatibility = configuration.RestrictBpmCompatibility;
            StripMixedInKeyPrefixes = configuration.StripMixedInKeyPrefixes;
        }

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

        public bool Close
        {
            get { return close; }
            set
            {
                close = value;
                RaisePropertyChanged(() => Close);
            }
        }
    }
}