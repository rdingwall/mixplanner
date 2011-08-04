using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Julana.CommandLine.DomainModel.MixingStrategies
{
    public class OneSemitoneEnergyBoost : IMixingStrategy
    {
        public bool CanProceed(Track currentTrack, Track nextTrack)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMixingStrategy
    {
        bool CanProceed(Track currentTrack, Track nextTrack);
    }
}
