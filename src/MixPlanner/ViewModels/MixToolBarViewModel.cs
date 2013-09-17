using System;
using GalaSoft.MvvmLight;
using MixPlanner.Commands;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public class MixToolBarViewModel : ViewModelBase
    {
        public IMix Mix { get; private set; }
        public SaveMixAsCommand SaveAsCommand { get; private set; }

        public MixToolBarViewModel(SaveMixAsCommand saveAsCommand, IMix mix)
        {
            if (saveAsCommand == null) throw new ArgumentNullException("saveAsCommand");
            if (mix == null) throw new ArgumentNullException("mix");
            SaveAsCommand = saveAsCommand;
            Mix = mix;
        }
    }
}