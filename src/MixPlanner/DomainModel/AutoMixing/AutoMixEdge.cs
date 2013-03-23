using QuickGraph;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixEdge<T> : Edge<T> where T : IAutoMixable
    {
        public IMixingStrategy Strategy { get; private set; }

        public AutoMixEdge(
            T source,
            T target,
            IMixingStrategy strategy)
            : base(source, target)
        {
            Strategy = strategy;
        }
    }
}