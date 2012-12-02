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

        public bool IsCompatible(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            return secondTrack.Key.HasSameScaleAs(firstTrack.Key)
                   && secondTrack.Key.Equals(firstTrack.Key.IncreasePitch(increaseAmount));
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