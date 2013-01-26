namespace MixPlanner.DomainModel
{
    public static class PitchFaderStep
    {
        // 2 steps of 0.03, i.e. +6% to -6% range
        public const double Value = 0.03;
        public const int NumberOfSteps = 2;
    }
}