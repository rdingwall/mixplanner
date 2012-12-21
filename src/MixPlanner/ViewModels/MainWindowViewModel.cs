using System;
using GalaSoft.MvvmLight;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MixViewModel Mix { get; private set; }
        public TrackLibraryViewModel TrackLibrary { get; private set; }
        public MiniPlayerViewModel MiniPlayer { get; private set; }

        public MainWindowViewModel(
            MixViewModel mixViewModel,
            TrackLibraryViewModel trackLibraryViewModel,
            MiniPlayerViewModel miniPlayerViewModel)
        {
            if (mixViewModel == null) throw new ArgumentNullException("mixViewModel");
            if (trackLibraryViewModel == null) throw new ArgumentNullException("trackLibraryViewModel");
            if (miniPlayerViewModel == null) throw new ArgumentNullException("miniPlayerViewModel");

            Mix = mixViewModel;
            TrackLibrary = trackLibraryViewModel;
            MiniPlayer = miniPlayerViewModel;
        }
    }
}