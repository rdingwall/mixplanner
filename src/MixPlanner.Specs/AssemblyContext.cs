using Machine.Specifications;
using log4net.Config;

namespace MixPlanner.Specs
{
    public class AssemblyContext : IAssemblyContext
    {
        public void OnAssemblyStart()
        {
            BasicConfigurator.Configure();
        }

        public void OnAssemblyComplete()
        {
        }
    }
}