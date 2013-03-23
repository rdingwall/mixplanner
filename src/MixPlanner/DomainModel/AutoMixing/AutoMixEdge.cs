using QuickGraph;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixEdge : Edge<AutoMixingBucket>
    {
        public IMixingStrategy Strategy { get; private set; }

        public AutoMixEdge(
            AutoMixingBucket source,
            AutoMixingBucket target,
            IMixingStrategy strategy)
            : base(source, target)
        {
            Strategy = strategy;
        }
    }
}