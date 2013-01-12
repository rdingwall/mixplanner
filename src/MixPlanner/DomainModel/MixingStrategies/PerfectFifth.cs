namespace MixPlanner.DomainModel.MixingStrategies
{
    public class PerfectFifth : IncreasePitchStrategyBase
    {
        public PerfectFifth(IBpmRangeChecker bpmRangeChecker) 
            : base(bpmRangeChecker, increaseAmount: 1) { }

        public override string Description
        {
            get { return "Perfect fifth (+1)"; }
        }
    }
}