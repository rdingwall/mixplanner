using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace MixPlanner.ViewModels
{
    public class MixSurroundingAreaViewModel : ViewModelBase
    {
        public MixViewModel Mix { get; private set; }

        public MixSurroundingAreaViewModel(MixViewModel mix, IMessenger messenger)
            : base(messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            Mix = mix;
        }


    }
}