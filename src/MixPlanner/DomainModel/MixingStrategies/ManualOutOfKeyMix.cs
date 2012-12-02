using System;
using System.Collections.Generic;

namespace MixPlanner.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualOutOfKeyMix : IMixingStrategy
    {
        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            return true;
        }

        public string Description { get { return "Out of key mix / train wreck!"; } }
    }
}