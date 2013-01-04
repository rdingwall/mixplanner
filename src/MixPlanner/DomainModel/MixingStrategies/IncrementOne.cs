namespace MixPlanner.DomainModel.MixingStrategies
{
    public class IncrementOne : IncreasePitchStrategyBase
    {
        public IncrementOne(IBpmRangeChecker bpmRangeChecker) 
            : base(bpmRangeChecker, increaseAmount: 1) { }
    }
}