using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MixPlanner.Specs.Extensions
{
    [TestFixture]
    public class DoubleExtensions
    {
        const double Tolerance = 0.00001;

        static IEnumerable<Tuple<double, double>> GetCases()
        {
            yield return Tuple.Create(-0.02, 0.0);
            yield return Tuple.Create(-0.03, -0.03);
            yield return Tuple.Create(-0.031, -0.03);
            yield return Tuple.Create(-0.041, -0.03);
            yield return Tuple.Create(-0.06, -0.06);
            yield return Tuple.Create(-0.061, -0.06);
            yield return Tuple.Create(-0.091, -0.09);
            yield return Tuple.Create(0.0, 0.0);
            yield return Tuple.Create(0.02, 0.0);
            yield return Tuple.Create(0.03, 0.03);
            yield return Tuple.Create(0.031, 0.03);
            yield return Tuple.Create(0.041, 0.03);
            yield return Tuple.Create(0.06, 0.06);
            yield return Tuple.Create(0.061, 0.06);
            yield return Tuple.Create(0.091, 0.09);
            }

        [Test, TestCaseSource("GetCases")]
        public void It_should_floor_to_the_previous_harmonic_change_interval(Tuple<double, double> p)
        {
            var rounded = p.Item1.FloorToNearest(0.03);
            Assert.That(rounded, Is.InRange(p.Item2 - Tolerance, p.Item2 + Tolerance));
        }
     }
}