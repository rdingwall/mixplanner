using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using NUnit.Framework;
using QuickGraph;
using SharpTestsEx;
using MoreLinq;

namespace MixPlanner.Specs.DomainModel
{
    public static class AssertExtensions
    {
        public static void ShouldAllHaveCountEqualTo<T>(
            this IEnumerable<IEnumerable<T>> collections, 
            int count)
        {
            foreach (var collection in collections)
                collection.Should().Have.Count.EqualTo(count);
        }

        public static void ShouldAllHaveCountEqualTo<T1, T2>(
            this IEnumerable<IEnumerable<T1>> collections, 
            IEnumerable<T2> other)
        {
            var expectedCount = other.Count();

            foreach (var collection in collections)
                collection.Should().Have.Count.EqualTo(expectedCount);
        }

        public static void ShouldAllHaveVertexCountEqualTo<T2>(
            this IEnumerable<IEnumerable<StrategyEdge>> collections,
            IEnumerable<T2> other)
        {
            var expectedCount = other.Count();

            foreach (var collection in collections)
                collection.GetHarmonicKeys().Should().Have.Count.EqualTo(expectedCount);
        }

        public static void ShouldNotContainDuplicates<T>(
            this IEnumerable<IEnumerable<T>> collections)
        {
            foreach (var collection in collections)
                collection.Should().Have.SameSequenceAs(collection.Distinct());
        }

        public static IEnumerable<HarmonicKey> GetPathStartingWith(
            this IEnumerable<IEnumerable<StrategyEdge>> paths, HarmonicKey key)
        {
            var path = paths.First(p => p.First().Source.ActualKey.Equals(key));
            return path.GetHarmonicKeys();
        }

        public static IEnumerable<HarmonicKey> GetHarmonicKeys(this IEnumerable<StrategyEdge> path)
        {
            return path.Select(e => e.Source.ActualKey).Concat(new[] {path.Last().Target.ActualKey});
        }

        public static void PrintPaths(this LongestPathAlgorithm<PlaybackSpeed, StrategyEdge> algo)
        {
            foreach (var path in algo.LongestPaths)
            {
                var vertices = path.Select(e => e.Source.ActualKey).Concat(new[] { path.Last().Target.ActualKey });
                Console.WriteLine("{0}", String.Join(" -> ", vertices));
            }
        }

        public static void ShouldAllBeValidTransitions(
            this IEnumerable<IEnumerable<StrategyEdge>> paths, 
            IEnumerable<IMixingStrategy> mixingStrategies)
        {
            foreach (IEnumerable<StrategyEdge> path in paths)
            {
                foreach (StrategyEdge edge in path)
                {
                    var isHarmonic = mixingStrategies.Any(s => s.IsCompatible(edge.Source, edge.Target));
                    if (!isHarmonic)
                    {
                        var mix = String.Join(" -> ", path.GetHarmonicKeys());
                        Assert.Fail("Mix {0} is not harmonic between edge {1} -> {2}",
                            mix, edge.Source.ActualKey, edge.Target.ActualKey);
                    }
                }
            }
        }
    }
}