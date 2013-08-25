using Machine.Specifications;
using NUnit.Framework;
using log4net.Config;

namespace MixPlanner.Specs
{
    [SetUpFixture]
    public class AssemblyContext : IAssemblyContext
    {
        [SetUp]
        public void OnAssemblyStart()
        {
            BasicConfigurator.Configure();
            TestDirectories.RecreateAll();
        }

        [TearDown]
        public void OnAssemblyComplete()
        {
            //TestDirectories.DeleteAll();
        }
    }
}