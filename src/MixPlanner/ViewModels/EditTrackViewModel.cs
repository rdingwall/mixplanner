using System;
using System.Collections.Generic;
using System.Windows.Input;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public class EditTrackViewModel : CloseableViewModelBase
    {
        string artist;
        string genre;
        string year;
        string label;
        string filePath;
        bool fileExists;
        double bpm;
        HarmonicKey harmonicKey;
        string title;

        public EditTrackViewModel(
            SaveTrackCommand saveCommand,
            CloseWindowCommand cancelCommand,
            Track track)
        {
            if (saveCommand == null) throw new ArgumentNullException("saveCommand");
            if (cancelCommand == null) throw new ArgumentNullException("cancelCommand");
            if (track == null) throw new ArgumentNullException("track");
            SaveCommand = saveCommand;
            Track = track;
            CloseCommand = cancelCommand;

            HarmonicKey = track.OriginalKey;
            Bpm = track.OriginalBpm;
            Artist = track.Artist;
            Title = track.Title;
            Genre = track.Genre;
            Year = track.Year;
            Label = track.Label;
            FilePath = track.Filename;
            AllHarmonicKeys = HarmonicKey.AllKeys;
        }

        public HarmonicKey HarmonicKey
        {
            get { return harmonicKey; }
            set
            {
                harmonicKey = value;
                RaisePropertyChanged(() => HarmonicKey);
            }
        }

        public double Bpm
        {
            get { return bpm; }
            set
            {
                bpm = value;
                RaisePropertyChanged(() => Bpm);
            }
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                RaisePropertyChanged(() => Artist);
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                RaisePropertyChanged(() => Year);
            }
        }

        public string Genre
        {
            get { return genre; }
            set
            {
                genre = value;
                RaisePropertyChanged(() => Genre);
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                RaisePropertyChanged(() => Label);
            }
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        public bool FileExists
        {
            get { return fileExists; }
            set
            {
                fileExists = value;
                RaisePropertyChanged(() => FileExists);
                RaisePropertyChanged(() => FileNotFound);
            }
        }

        public bool FileNotFound { get { return !FileExists; } }

        public IEnumerable<HarmonicKey> AllHarmonicKeys { get; private set; } 

        public ICommand BrowseCommand { get; private set; }
        public SaveTrackCommand SaveCommand { get; private set; }
        public CloseWindowCommand CloseCommand { get; private set; }

        public Track Track { get; private set; }
    }
}