namespace MixPlanner.DomainModel.MixingStrategies
{
    public class AdvanceOne : IncreasePitchStrategyBase
    {
        public AdvanceOne(IBpmRangeChecker bpmRangeChecker) 
            : base(bpmRangeChecker, increaseAmount: 1) { }

        public override string Description
        {
            get { return "Advance one (+1)"; }
        }
    }
}