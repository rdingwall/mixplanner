namespace MixPlanner.DomainModel
{
    public class Transition
    {
        public HarmonicKey FromKey { get; set; }
        public HarmonicKey ToKey { get; set; }
        public IMixingStrategy Strategy { get; set; }
        public string Description { get; set; }
    }
}