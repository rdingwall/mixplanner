namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    // http://www.harmonic-mixing.com/EnergyBoostMixing.aspx
    public class OneSemitoneEnergyBoost : IncreasePitchStrategyBase
    {
        public OneSemitoneEnergyBoost() : base(increaseAmount: 7) { }
    }
}
