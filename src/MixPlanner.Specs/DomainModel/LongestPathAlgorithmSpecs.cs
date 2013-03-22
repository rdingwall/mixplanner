using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using QuickGraph;
using SharpTestsEx;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(LongestPathAlgorithm<,>))]
    public class LongestPathAlgorithmSpecs
    {
        public class When_there_were_multiple_valid_paths
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithMultiplePaths;
                    testCase.PrintTracks();

                    var strategies = TestMixingStrategies.PreferredStrategies;

                    var edges =
                        from startKey in testCase.Tracks
                        from endKey in testCase.Tracks
                        where !endKey.Equals(startKey)
                        from strategy in strategies
                        where strategy.IsCompatible(endKey, startKey)
                        orderby startKey.ActualKey, endKey.ActualKey
                        select new StrategyEdge(endKey, startKey, strategy);

                    graph = new AdjacencyGraph<PlaybackSpeed, StrategyEdge>();
                    graph.AddVertexRange(testCase.Tracks.Distinct());
                    graph.AddEdgeRange(edges);

                    Console.WriteLine("{0} edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);
                };

            Because of =
                () =>
                {
                    var sw = Stopwatch.StartNew();
                    var algo = new LongestPathAlgorithm<PlaybackSpeed, StrategyEdge>(graph);
                    //algo.SetRootVertex(graph.Vertices.Single(v => v.ActualKey.Equals(HarmonicKey.Key11A)));
                    algo.Compute();
                    sw.Stop();
                    Console.WriteLine("Elapsed: {0}", sw.Elapsed);
                    paths = algo.LongestPaths;
                    algo.PrintPaths();
                };

            It should_find_some_paths = () => paths.Should().Not.Be.Empty();

            It should_only_return_paths_that_visit_every_vertices =
                () => paths.ShouldAllHaveVertexCountEqualTo(testCase.Tracks);

            It should_not_contain_any_cycles =
                () => paths.ShouldNotContainDuplicates();

            It should_find_the_expected_path_starting_at_key_11a =
                () => paths.GetPathStartingWith(HarmonicKey.Key11A)
                    .Should().Have.SameSequenceAs(testCase.ExpectedPaths[HarmonicKey.Key11A]);

            It should_find_the_expected_path_starting_at_key_1a =
                () => paths.GetPathStartingWith(HarmonicKey.Key1A)
                    .Should().Have.SameSequenceAs(testCase.ExpectedPaths[HarmonicKey.Key1A]);

            It should_find_the_expected_path_starting_at_key_6a =
                () => paths.GetPathStartingWith(HarmonicKey.Key6A)
                    .Should().Have.SameSequenceAs(testCase.ExpectedPaths[HarmonicKey.Key6A]);

            static IEnumerable<IEnumerable<StrategyEdge>> paths;
            static AdjacencyGraph<PlaybackSpeed, StrategyEdge> graph;
            static LongestPathAlgorithmTestCase testCase;
        }

        public class When_there_were_no_paths
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithNoPaths;
                    testCase.PrintTracks();

                    var strategies = TestMixingStrategies.PreferredStrategies;

                    var edges =
                        from startKey in testCase.Tracks
                        from endKey in testCase.Tracks
                        where !endKey.Equals(startKey)
                        from strategy in strategies
                        where strategy.IsCompatible(endKey, startKey)
                        orderby startKey.ActualKey, endKey.ActualKey
                        select new StrategyEdge(endKey, startKey, strategy);

                    graph = new AdjacencyGraph<PlaybackSpeed, StrategyEdge>();
                    graph.AddVertexRange(testCase.Tracks.Distinct());
                    graph.AddEdgeRange(edges);

                    Console.WriteLine("{0} edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);
                };

            Because of =
                () =>
                {
                    var sw = Stopwatch.StartNew();
                    var algo = new LongestPathAlgorithm<PlaybackSpeed, StrategyEdge>(graph);
                    //algo.SetRootVertex(graph.Vertices.Single(v => v.ActualKey.Equals(HarmonicKey.Key11A)));
                    algo.Compute();
                    sw.Stop();
                    Console.WriteLine("Elapsed: {0}", sw.Elapsed);
                    paths = algo.LongestPaths;
                    algo.PrintPaths();
                };

            It should_not_return_any_paths = () => paths.Should().Be.Empty();

            static IEnumerable<IEnumerable<StrategyEdge>> paths;
            static AdjacencyGraph<PlaybackSpeed, StrategyEdge> graph;
            static LongestPathAlgorithmTestCase testCase;
        }

        public class When_there_was_only_a_single_edge
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithSingleEdge;
                    testCase.PrintTracks();

                    var strategies = TestMixingStrategies.PreferredStrategies;

                    var edges =
                        from startKey in testCase.Tracks
                        from endKey in testCase.Tracks
                        where !endKey.Equals(startKey)
                        from strategy in strategies
                        where strategy.IsCompatible(endKey, startKey)
                        orderby startKey.ActualKey, endKey.ActualKey
                        select new StrategyEdge(endKey, startKey, strategy);

                    graph = new AdjacencyGraph<PlaybackSpeed, StrategyEdge>();
                    graph.AddVertexRange(testCase.Tracks.Distinct());
                    graph.AddEdgeRange(edges);

                    Console.WriteLine("{0} edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);
                };

            Because of =
                () =>
                {
                    var sw = Stopwatch.StartNew();
                    var algo = new LongestPathAlgorithm<PlaybackSpeed, StrategyEdge>(graph);
                    //algo.SetRootVertex(graph.Vertices.Single(v => v.ActualKey.Equals(HarmonicKey.Key11A)));
                    algo.Compute();
                    sw.Stop();
                    Console.WriteLine("Elapsed: {0}", sw.Elapsed);
                    paths = algo.LongestPaths;
                    algo.PrintPaths();
                };

            It should_find_one_path = () => paths.Should().Have.Count.EqualTo(1);

            It should_only_return_paths_that_visit_every_vertices =
                () => paths.ShouldAllHaveVertexCountEqualTo(testCase.Tracks);

            It should_not_contain_any_cycles =
                () => paths.ShouldNotContainDuplicates();

            It should_find_the_expected_path_starting_at_key_1a =
                () => paths.GetPathStartingWith(HarmonicKey.Key1A)
                    .Should().Have.SameSequenceAs(testCase.ExpectedPaths[HarmonicKey.Key1A]);

            static IEnumerable<IEnumerable<StrategyEdge>> paths;
            static AdjacencyGraph<PlaybackSpeed, StrategyEdge> graph;
            static LongestPathAlgorithmTestCase testCase;
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