namespace MixPlanner.DomainModel.MixingStrategies
{
    public class PerfectFourth : IncreasePitchStrategyBase
    {
        public PerfectFourth(IBpmRangeChecker bpmRangeChecker) 
            : base(bpmRangeChecker, increaseAmount: -1) { }

        public override string Description
        {
            get { return "Perfect fourth (-1)"; }
        }
    }
}