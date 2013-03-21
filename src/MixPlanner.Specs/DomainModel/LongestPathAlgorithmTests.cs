using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;
using NUnit.Framework;
using QuickGraph;
using SharpTestsEx;

namespace MixPlanner.Specs.DomainModel
{
    // Need to refactor this test a bit
    public class LongestPathAlgorithmTests
    {
        [Test]
        public void Test()
        {
            var random = new Random();
            var keys = new[]
                           {
                               HarmonicKey.Key1A,
                               HarmonicKey.Key2A,
                               HarmonicKey.Key3A,
                               HarmonicKey.Key4A,
                               HarmonicKey.Key6A,
                               HarmonicKey.Key7A,
                               HarmonicKey.Key8A,
                               HarmonicKey.Key11A,
                           }
                           //.Concat(TestMixes.GetRandomMix(1000).Items.Select(i => i.PlaybackSpeed.ActualKey))
                           .Distinct()
                           .OrderBy(_ => random.Next())
                           .Select(k => new PlaybackSpeed(k, 128));

            Console.WriteLine(String.Join(", ", keys.Select(k => k.ActualKey).OrderBy(k => k)));

            var bpmRangeChecker = new BpmRangeChecker();
            var strategies = new IMixingStrategy[]
                {
                    //new SameKey(bpmRangeChecker), 
                    new OneSemitoneEnergyBoost(bpmRangeChecker),
                    new PerfectFifth(bpmRangeChecker),
                    new TwoSemitoneEnergyBoost(bpmRangeChecker),
                    new RelativeMinor(bpmRangeChecker),
                    new RelativeMajor(bpmRangeChecker) 
                };

            var edges =
                from startKey in keys
                from endKey in keys
                where !endKey.Equals(startKey)
                from strategy in strategies
                where strategy.IsCompatible(endKey, startKey)
                orderby startKey.ActualKey, endKey.ActualKey
                select new StrategyEdge(endKey, startKey, strategy);

            var graph = new AdjacencyGraph<PlaybackSpeed, StrategyEdge>();
            graph.AddVertexRange(keys.Distinct());
            graph.AddEdgeRange(edges);

            Console.WriteLine("{0} edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);

            var sw = Stopwatch.StartNew();

            var algo = new LongestPathAlgorithm<PlaybackSpeed, StrategyEdge>(graph);
            //algo.SetRootVertex(graph.Vertices.Single(v => v.ActualKey.Equals(HarmonicKey.Key11A)));
            algo.Compute();

            sw.Stop();
            Console.WriteLine("Elapsed: {0}", sw.Elapsed);

            PrintPaths(algo);

            algo.LongestPaths.Should().Have.Count.EqualTo(4);

            GetPathStartingWith(algo.LongestPaths, HarmonicKey.Key11A)
                .Should().Have.SameSequenceAs(new[]
                                                  {
                                                      HarmonicKey.Key11A,
                                                      HarmonicKey.Key1A,
                                                      HarmonicKey.Key2A,
                                                      HarmonicKey.Key3A,
                                                      HarmonicKey.Key4A,
                                                      HarmonicKey.Key6A,
                                                      HarmonicKey.Key7A,
                                                      HarmonicKey.Key8A
                                                  });

            GetPathStartingWith(algo.LongestPaths, HarmonicKey.Key6A)
                .Should().Have.SameSequenceAs(new[]
                                                  {
                                                      HarmonicKey.Key6A, 
                                                      HarmonicKey.Key7A, 
                                                      HarmonicKey.Key2A, 
                                                      HarmonicKey.Key3A, 
                                                      HarmonicKey.Key4A, 
                                                      HarmonicKey.Key11A, 
                                                      HarmonicKey.Key1A, 
                                                      HarmonicKey.Key8A
                                                  });

            GetPathStartingWith(algo.LongestPaths, HarmonicKey.Key1A)
                .Should().Have.SameSequenceAs(new[]
                                                  {
                                                      HarmonicKey.Key1A, 
                                                      HarmonicKey.Key2A, 
                                                      HarmonicKey.Key3A, 
                                                      HarmonicKey.Key4A, 
                                                      HarmonicKey.Key11A,
                                                      HarmonicKey.Key6A, 
                                                      HarmonicKey.Key7A, 
                                                      HarmonicKey.Key8A
                                                  });

        }

        static void PrintPaths(LongestPathAlgorithm<PlaybackSpeed, StrategyEdge> algo)
        {
            foreach (var path in algo.LongestPaths)
            {
                var vertices = path.Select(e => e.Source.ActualKey).Concat(new[] {path.Last().Target.ActualKey});
                Console.WriteLine("{0}", String.Join(" -> ", vertices));
            }
        }

        IEnumerable<HarmonicKey> GetPathStartingWith(IEnumerable<IEnumerable<StrategyEdge>> paths,
                                                     HarmonicKey key)
        {
            var path = paths.First(p => p.First().Source.ActualKey.Equals(key));
            return path.Select(e => e.Source.ActualKey).Concat(new[] {path.Last().Target.ActualKey});
        }
    }

    public class StrategyEdge : Edge<PlaybackSpeed>
    {
        public IMixingStrategy Strategy { get; private set; }

        public StrategyEdge(
            PlaybackSpeed source,
            PlaybackSpeed target,
            IMixingStrategy strategy)
            : base(source, target)
        {
            Strategy = strategy;
        }
    }
}