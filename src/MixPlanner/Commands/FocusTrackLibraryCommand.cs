using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class FocusTrackLibraryCommand : RequestFocusCommandBase<TrackLibraryViewModel>
    {
        public FocusTrackLibraryCommand(IDispatcherMessenger messenger) : base(messenger)
        {
        }
    }
}