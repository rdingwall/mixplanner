namespace MixPlanner.DomainModel.MixingStrategies
{
    // http://www.harmonic-mixing.com/EnergyBoostMixing.aspx
    public class OneSemitone : IncreasePitchStrategyBase
    {
        public OneSemitone(IBpmRangeChecker bpmRangeChecker) 
            : base(bpmRangeChecker, increaseAmount: 7) { }

        public override string Description
        {
            get { return "One semitone (+7)"; }
        }
    }
}
