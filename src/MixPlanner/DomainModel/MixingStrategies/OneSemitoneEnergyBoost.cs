﻿namespace MixPlanner.DomainModel.MixingStrategies
{
    // http://www.harmonic-mixing.com/EnergyBoostMixing.aspx
    public class OneSemitoneEnergyBoost : IncreasePitchStrategyBase
    {
        public OneSemitoneEnergyBoost(IBpmRangeChecker bpmRangeChecker) 
            : base(bpmRangeChecker, increaseAmount: 7) { }

        public override string Description
        {
            get { return "One semitone energy boost (+7)"; }
        }
    }
}
