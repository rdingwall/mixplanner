namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class OneSemitoneEnergyBoost : IncreasePitchStrategyBase
    {
        public OneSemitoneEnergyBoost() : base(increaseAmount: 7) { }
    }
}
