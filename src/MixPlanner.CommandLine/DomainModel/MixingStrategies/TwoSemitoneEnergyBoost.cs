namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class TwoSemitoneEnergyBoost : IncreasePitchStrategyBase
    {
        public TwoSemitoneEnergyBoost() : base(increaseAmount: 7 * 2) { }
    }
}