namespace MixPlanner.DomainModel.MixingStrategies
{
    public class OneSemitoneDecrease : IncreasePitchStrategyBase
    {
        public OneSemitoneDecrease(IBpmRangeChecker bpmRangeChecker)
            : base(bpmRangeChecker, increaseAmount: -7) { }

        public override string Description
        {
            get { return "One-semitone decrease (-7)"; }
        } 
    }
}