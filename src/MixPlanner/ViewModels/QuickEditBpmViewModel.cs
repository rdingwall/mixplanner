using System;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MvvmValidation;

namespace MixPlanner.ViewModels
{
    public class QuickEditBpmViewModel : ValidatableViewModelBase
    {
        string bpm;

        public SaveBpmCommand SaveBpmCommand { get; private set; }
        public CloseWindowCommand CloseCommand { get; private set; }
        public Track Track { get; private set; }

        public string Bpm
        {
            get { return bpm; }
            set
            {
                bpm = value;
                RaisePropertyChanged(() => Bpm);
            }
        }

        public QuickEditBpmViewModel(
            Track track, 
            SaveBpmCommand saveBpmCommand, 
            CloseWindowCommand closeCommand)
        {
            if (track == null) throw new ArgumentNullException("track");
            if (saveBpmCommand == null) throw new ArgumentNullException("saveBpmCommand");
            if (closeCommand == null) throw new ArgumentNullException("closeCommand");

            Validator.AddRule(() => Bpm, ValidateBpm);

            Track = track;
            SaveBpmCommand = saveBpmCommand;
            CloseCommand = closeCommand;

            if (!Double.IsNaN(track.OriginalBpm))
                bpm = track.OriginalBpm.ToString();
        }

        RuleResult ValidateBpm()
        {
            double result;
            if (!Double.TryParse(bpm, out result) || Double.IsNaN(result))
                return RuleResult.Invalid("Invalid BPM");

            return RuleResult.Valid();
        }
    }
}