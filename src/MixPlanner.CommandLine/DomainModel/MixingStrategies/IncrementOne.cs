namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class IncrementOne : IncreasePitchStrategyBase
    {
        public IncrementOne() : base(increaseAmount: 1) {}
    }
}