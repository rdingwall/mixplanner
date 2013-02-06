using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.MixingStrategies;
using NUnit.Framework;
using QuickGraph;
using QuickGraph.Algorithms;

namespace MixPlanner.Specs
{
    public class GraphTests
    {
        [Test]
        public void Test()
        {
            var keys = new[]
                           {
                               HarmonicKey.Key2A,
                               HarmonicKey.Key3A,
                               HarmonicKey.Key5A,
                               HarmonicKey.Key8A,
                               HarmonicKey.Key8A,
                               HarmonicKey.Key8A,
                               HarmonicKey.Key10A,
                               HarmonicKey.Key11B
                           }.Select(k => new PlaybackSpeed(k, 128));

            var bpmRangeChecker = new BpmRangeChecker();
            var strategies = new IMixingStrategy[]
                {
                    new SameKey(bpmRangeChecker), 
                    new OneSemitone(bpmRangeChecker),
                    new PerfectFifth(bpmRangeChecker),
                    new TwoSemitoneEnergyBoost(bpmRangeChecker)
                };

            var edges =
                from startKey in keys
                from endKey in keys
                where !endKey.Equals(startKey)
                from strategy in strategies
                where strategy.IsCompatible(endKey, startKey)
                select new StrategyEdge(endKey, startKey, strategy);

            var graph = new AdjacencyGraph<PlaybackSpeed, StrategyEdge>();
            graph.AddVertexRange(keys.Distinct());
            graph.AddEdgeRange(edges);

            Console.WriteLine("{0} edges, {1} vertices.", 
                graph.EdgeCount, graph.VertexCount);

            var pathCount = EulerianTrailAlgorithm<PlaybackSpeed, StrategyEdge>.ComputeEulerianPathCount(graph);
            Console.WriteLine("Found {0} eulerian paths.", pathCount);

            var algo = new EulerianTrailAlgorithm<PlaybackSpeed, StrategyEdge>(graph);

            algo.AddTemporaryEdges((source, target) => new StrategyEdge(source, target, null));
            algo.Compute();
            var trails = algo.Trails();
            algo.RemoveTemporaryEdges();

            Console.WriteLine("Found {0} eulerian trails.", trails.Count);

            foreach (ICollection<StrategyEdge> trail in trails)
            {
                
            }
        }

        class StrategyEdge : Edge<PlaybackSpeed>
        {
            public IMixingStrategy Strategy { get; private set; }

            public StrategyEdge(
                PlaybackSpeed source, 
                PlaybackSpeed target, 
                IMixingStrategy strategy) : base(source, target)
            {
                Strategy = strategy;
            }
        }
    }
}