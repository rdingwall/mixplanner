namespace MixPlanner.DomainModel
{
    public class UnknownKey : HarmonicKey
    {
        public UnknownKey() : base(0, Scale.Unknown, -1) {}

        public override string ToString()
        {
            return "Unknown";
        }

        public override HarmonicKey Advance(int value)
        {
            return this;
        }

        public override HarmonicKey ToMajor()
        {
            return this;
        }

        public override HarmonicKey ToMinor()
        {
            return this;
        }
    }
}