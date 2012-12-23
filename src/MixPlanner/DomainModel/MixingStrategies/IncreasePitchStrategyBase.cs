using System;

namespace MixPlanner.DomainModel.MixingStrategies
{
    public abstract class IncreasePitchStrategyBase : IMixingStrategy
    {
        readonly int increaseAmount;

        protected IncreasePitchStrategyBase(int increaseAmount)
        {
            this.increaseAmount = increaseAmount;
        }

        public bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var firstKey = first.ActualEndingKey;
            var secondKey = second.ActualStartingKey;

            return secondKey.HasSameScaleAs(firstKey)
                   && secondKey.Equals(firstKey.IncreasePitch(increaseAmount));
        }

        public virtual string Description
        {
            get
            {
                return String.Format("Increase pitch by {0}", increaseAmount);
            }
        }
    }
}