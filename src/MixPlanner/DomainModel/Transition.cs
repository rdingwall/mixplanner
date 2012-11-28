using System;

namespace MixPlanner.DomainModel
{
    public class Transition
    {
        public Transition(HarmonicKey toKey, IMixingStrategy strategy) : this(null, strategy, toKey)
        {
        }

        public Transition(HarmonicKey fromKey, IMixingStrategy strategy, HarmonicKey toKey)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (toKey == null) throw new ArgumentNullException("toKey");
            FromKey = fromKey;
            Strategy = strategy;
            ToKey = toKey;
        }

        public HarmonicKey FromKey { get; private set; }
        public HarmonicKey ToKey { get; private set; }
        public IMixingStrategy Strategy { get; private set; }
        public string Description { get { return Strategy.Description; }}
    }
}