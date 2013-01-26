using System;
using System.Collections.Generic;
using Castle.Windsor;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Loader;
using NUnit.Framework;

namespace MixPlanner.Specs.Loader
{
    [TestFixture]
    public class HarmonicKeySuperParserTests
    {
        IHarmonicKeySuperParser parser;
        IWindsorContainer container;

        public static IEnumerable<Tuple<string, HarmonicKey>> GetCases()
        {
            return new []
                        {
                            Tuple.Create("12A", HarmonicKey.Key12A),
                            Tuple.Create("12a", HarmonicKey.Key12A),
                            Tuple.Create("D-Flat Minor", HarmonicKey.Key12A),
                            Tuple.Create(" d-flat minor ", HarmonicKey.Key12A),
                            Tuple.Create("C-Sharp Minor", HarmonicKey.Key12A),
                            Tuple.Create("dbm", HarmonicKey.Key12A),
                            Tuple.Create(" DBm", HarmonicKey.Key12A),
                            Tuple.Create("C#m", HarmonicKey.Key12A),
                            Tuple.Create("c#m ", HarmonicKey.Key12A),
                            Tuple.Create(" C♯ Minor ", HarmonicKey.Key12A),
                            Tuple.Create(" c♯ Minor ", HarmonicKey.Key12A),
                            Tuple.Create("  D♭ Minor ", HarmonicKey.Key12A),
                            Tuple.Create("5m", HarmonicKey.Key12A),
                            Tuple.Create("5M", HarmonicKey.Key12A),
                            Tuple.Create("dfsdf", HarmonicKey.Unknown),
                            Tuple.Create(" ", HarmonicKey.Unknown),
                            Tuple.Create((string)null, HarmonicKey.Unknown)
                        };

        }

        [Test, TestCaseSource("GetCases")]
        public void It_should_correctly_parse_the_key(Tuple<string, HarmonicKey> @case)
        {
            parser.ParseHarmonicKey(@case.Item1).ShouldEqual(@case.Item2);
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            container = new WindsorContainer();
            container.Install(new IocRegistrations());
            parser = container.Resolve<IHarmonicKeySuperParser>();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            container.Dispose();
        }
    }
}