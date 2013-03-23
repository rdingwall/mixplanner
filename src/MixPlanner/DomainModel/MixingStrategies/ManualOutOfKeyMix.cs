namespace MixPlanner.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualOutOfKeyMix : CompatibleBpmMixingStrategyBase
    {
        public ManualOutOfKeyMix(IBpmRangeChecker bpmRangeChecker) : base(bpmRangeChecker)
        {
        }

        public override bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey)
        {
            return true;
        }

        public override string Description { get { return "Out of key mix / train wreck!"; } }
    }
}