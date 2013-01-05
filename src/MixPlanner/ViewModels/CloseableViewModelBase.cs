using GalaSoft.MvvmLight;

namespace MixPlanner.ViewModels
{
    public abstract class CloseableViewModelBase : ViewModelBase
    {
        bool close;

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