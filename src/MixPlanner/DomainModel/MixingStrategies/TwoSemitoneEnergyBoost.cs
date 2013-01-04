namespace MixPlanner.DomainModel.MixingStrategies
{
    public class TwoSemitoneEnergyBoost : IncreasePitchStrategyBase
    {
        public TwoSemitoneEnergyBoost(IBpmRangeChecker bpmRangeChecker)
            : base(bpmRangeChecker, increaseAmount: 7 * 2) { }

        public override string Description
        {
            get { return "Two semitone energy boost (+2)"; }
        } 
    }
}