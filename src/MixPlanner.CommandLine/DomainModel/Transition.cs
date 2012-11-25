using System;

namespace MixPlanner.CommandLine.DomainModel
{
    public class Transition
    {
        public Transition(Key toKey, IMixingStrategy strategy) : this(null, strategy, toKey)
        {
        }

        public Transition(Key fromKey, IMixingStrategy strategy, Key toKey)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (toKey == null) throw new ArgumentNullException("toKey");
            FromKey = fromKey;
            Strategy = strategy;
            ToKey = toKey;
        }

        public Key FromKey { get; private set; }
        public Key ToKey { get; private set; }
        public IMixingStrategy Strategy { get; private set; }
        public string Description { get { return Strategy.Description; }}
    }
}