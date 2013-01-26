namespace MixPlanner.DomainModel.MixingStrategies
{
    public class TwoSemitoneDecrease : IncreasePitchStrategyBase
    {
        public TwoSemitoneDecrease(IBpmRangeChecker bpmRangeChecker)
            : base(bpmRangeChecker, increaseAmount: 7 * -2) { }

        public override string Description
        {
            get { return "Two-semitone decrease (-2)"; }
        } 
    }
}