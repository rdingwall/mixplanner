using System;
using System.Collections.Generic;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public class QuickEditHarmonicKeyViewModel : CloseableViewModelBase
    {
        HarmonicKey harmonicKey;

        public SaveHarmonicKeyCommand SaveBpmCommand { get; private set; }
        public CloseWindowCommand CloseCommand { get; private set; }
        public Track Track { get; private set; }

        public HarmonicKey HarmonicKey
        {
            get { return harmonicKey; }
            set
            {
                harmonicKey = value;
                RaisePropertyChanged(() => HarmonicKey);
            }
        }

        public IEnumerable<HarmonicKey> AllHarmonicKeys { get; private set; }

        public QuickEditHarmonicKeyViewModel(
            Track track, 
            SaveHarmonicKeyCommand saveBpmCommand, 
            CloseWindowCommand closeCommand)
        {
            if (track == null) throw new ArgumentNullException("track");
            if (saveBpmCommand == null) throw new ArgumentNullException("saveBpmCommand");
            if (closeCommand == null) throw new ArgumentNullException("closeCommand");

            AllHarmonicKeys = HarmonicKey.AllKeys;

            Track = track;
            SaveBpmCommand = saveBpmCommand;
            CloseCommand = closeCommand;

            harmonicKey = track.OriginalKey;
        }
    }
}