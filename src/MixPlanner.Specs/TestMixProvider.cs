using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public class TestMixProvider : ICurrentMixProvider
    {
        readonly IMix mix;

        public TestMixProvider(IMix mix)
        {
            this.mix = mix;
        }

        public IMix GetCurrentMix()
        {
            return mix;
        }
    }
}