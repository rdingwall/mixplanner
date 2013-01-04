namespace MixPlanner.DomainModel.MixingStrategies
{
    // aka trainwreck
    public class ManualOutOfKeyMix : CompatibleBpmMixingStrategyBase
    {
        public ManualOutOfKeyMix(IBpmRangeChecker bpmRangeChecker) : base(bpmRangeChecker)
        {
        }

        protected override bool IsCompatibleKey(PlaybackSpeed first, PlaybackSpeed second)
        {
            return true;
        }

        public override string Description { get { return "Out of key mix / train wreck!"; } }
    }
}