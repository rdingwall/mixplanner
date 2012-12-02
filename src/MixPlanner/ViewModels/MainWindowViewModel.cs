using System;
using GalaSoft.MvvmLight;

namespace MixPlanner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MixViewModel Mix { get; private set; }
        public TrackLibraryViewModel TrackLibrary { get; private set; }

        public MainWindowViewModel(
            MixViewModel mixViewModel,
            TrackLibraryViewModel trackLibraryViewModel
            )
        {
            if (mixViewModel == null) throw new ArgumentNullException("mixViewModel");

            Mix = mixViewModel;
            TrackLibrary = trackLibraryViewModel;
        }
    }
}