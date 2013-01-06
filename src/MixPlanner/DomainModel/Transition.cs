using System;

namespace MixPlanner.DomainModel
{
    public class Transition
    {
        public static Transition Intro(HarmonicKey toKey)
        {
            if (toKey == null) throw new ArgumentNullException("toKey");
            return new Transition {ToKey = toKey};
        }

        public static Transition Outro(HarmonicKey fromKey)
        {
            if (fromKey == null) throw new ArgumentNullException("fromKey");
            return new Transition { FromKey = fromKey };
        }

        private Transition() {}

        public Transition(
            HarmonicKey fromKey, 
            HarmonicKey toKey, 
            IMixingStrategy strategy, 
            double increaseRequired = 0)
        {
            if (fromKey == null) throw new ArgumentNullException("fromKey");
            if (toKey == null) throw new ArgumentNullException("toKey");
            if (strategy == null) throw new ArgumentNullException("strategy");
            FromKey = fromKey;
            ToKey = toKey;
            Strategy = strategy;
            IncreaseRequired = increaseRequired;
        }

        public HarmonicKey FromKey { get; private set; }
        public HarmonicKey ToKey { get; private set; }
        public IMixingStrategy Strategy { get; private set; }
        public double IncreaseRequired { get; private set; }
    }
}