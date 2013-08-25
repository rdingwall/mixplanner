namespace MixPlanner.DomainModel.MixingStrategies
{
    public abstract class MixingStrategyBase : IMixingStrategy
    {
        public abstract bool IsCompatible(PlaybackSpeed first, PlaybackSpeed second);
        public abstract bool IsCompatible(HarmonicKey firstKey, HarmonicKey secondKey);

        public abstract string Description { get; }

        public override string ToString()
        {
            return Description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType();
        }
    }
}