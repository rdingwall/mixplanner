using System;
using System.Collections.Generic;
using System.Windows.Input;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public class EditTrackViewModel : CloseableViewModelBase
    {
        readonly Track track;
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
            CloseWindowCommand closeWindowCommand,
            Track track)
        {
            if (closeWindowCommand == null) throw new ArgumentNullException("closeWindowCommand");
            if (track == null) throw new ArgumentNullException("track");
            this.track = track;
            CloseWindowCommand = closeWindowCommand;

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
                RaisePropertyChanged(() => HarmonicKey);
                harmonicKey = value;
            }
        }

        public double Bpm
        {
            get { return bpm; }
            set
            {
                RaisePropertyChanged(() => Bpm);
                bpm = value;
            }
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                RaisePropertyChanged(() => Artist);
                artist = value;
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                RaisePropertyChanged(() => Title);
                title = value;
            }
        }

        public string Year
        {
            get { return year; }
            set
            {
                RaisePropertyChanged(() => Year);
                year = value;
            }
        }

        public string Genre
        {
            get { return genre; }
            set
            {
                RaisePropertyChanged(() => Genre);
                genre = value;
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                RaisePropertyChanged(() => Label);
                label = value;
            }
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                RaisePropertyChanged(() => FilePath);
                filePath = value;
            }
        }

        public bool FileExists
        {
            get { return fileExists; }
            set
            {
                RaisePropertyChanged(() => FileExists);
                RaisePropertyChanged(() => FileNotFound);
                fileExists = value;
            }
        }

        public bool FileNotFound { get { return !FileExists; } }

        public IEnumerable<HarmonicKey> AllHarmonicKeys { get; private set; } 

        public ICommand BrowseCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public CloseWindowCommand CloseWindowCommand { get; set; }
    }
}