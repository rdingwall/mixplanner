using System;
using System.Diagnostics;
using QuickGraph;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixEdge : Edge<AutoMixingBucket>
    {
        public IMixingStrategy Strategy { get; private set; }
        public double Cost { get; private set; }

        public AutoMixEdge(
            AutoMixingBucket source, 
            AutoMixingBucket target, 
            IMixingStrategy strategy, 
            double cost)
            : base(source, target)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");
            Strategy = strategy;
            Cost = cost;
        }

    }
}