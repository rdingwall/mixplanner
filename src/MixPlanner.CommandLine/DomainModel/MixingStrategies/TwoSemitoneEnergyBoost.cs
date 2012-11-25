namespace MixPlanner.CommandLine.DomainModel.MixingStrategies
{
    public class TwoSemitoneEnergyBoost : IncreasePitchStrategyBase
    {
        public TwoSemitoneEnergyBoost() : base(increaseAmount: 7 * 2) { }

        public override string Description
        {
            get { return "Two semitone energy boost (+2)"; }
        } 
    }
}