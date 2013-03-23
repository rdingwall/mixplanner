using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QuickGraph;
using log4net;

namespace MixPlanner.DomainModel.AutoMixing
{
    public interface IAutoMixingStrategy
    {
        AutoMixingResult AutoMix(AutoMixingContext context);
    }

    public class AutoMixingStrategy : IAutoMixingStrategy
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (AutoMixingStrategy)); 
        readonly IMixingStrategiesFactory strategiesFactory;
        readonly IEdgeCostCalculator edgeCostCalculator;

        public AutoMixingStrategy(
            IMixingStrategiesFactory strategiesFactory,
            IEdgeCostCalculator edgeCostCalculator)
        {
            if (strategiesFactory == null) throw new ArgumentNullException("strategiesFactory");
            if (edgeCostCalculator == null) throw new ArgumentNullException("edgeCostCalculator");
            this.strategiesFactory = strategiesFactory;
            this.edgeCostCalculator = edgeCostCalculator;
        }

        public AutoMixingResult AutoMix(AutoMixingContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            
            List<IMixItem> unknownTracks = context.TracksToMix.Where(c => c.IsUnknownKeyOrBpm).ToList();
            IEnumerable<IMixItem> mixableTracks = context.TracksToMix.Except(unknownTracks);

            var groups = mixableTracks.GroupByBpmRange();

            Log.DebugFormat("{0} mixable tracks, {1} bpm range, {2} unmixable tracks (unknown key/BPM)",
                mixableTracks.Count(), groups.Count(), unknownTracks);

            var mixedTracks = new List<IMixItem>();
            var failedTracks = new List<IMixItem>();

            bool success = false;

            foreach (IGrouping<BpmRange, IMixItem> group in groups)
            {
                Log.DebugFormat("Auto-mixing BPM range {0}", group.Key);

                IEnumerable<IMixItem> mixed;
                if (TryAutoMixBpmGroup(context, group, out mixed))
                {
                    success = true;
                    mixedTracks.AddRange(mixed);
                }
                else
                    failedTracks.AddRange(group);
            }

            if (!success)
            {
                Log.Debug("Failed to find any auto mix paths. Returning unmodified tracks.");
                return AutoMixingResult.Failure(context);
            }

            return AutoMixingResult.Success(context, mixedTracks, failedTracks.Concat(unknownTracks));
        }

        bool TryAutoMixBpmGroup(AutoMixingContext context, 
            IEnumerable<IMixItem> tracks, out IEnumerable<IMixItem> mixedTracks)
        {
            if (tracks.Count() == 1)
            {
                mixedTracks = tracks;
                return true;
            }

            AutoMixingBucket optionalStartVertex;
            AdjacencyGraph<AutoMixingBucket, AutoMixEdge> graph
                = BuildGraph(tracks, context, out optionalStartVertex);

            AddPreferredEdges(graph);

            IEnumerable<AutoMixingBucket> mixedBuckets = GetPreferredMix(graph,
                                                                         optionalStartVertex,
                                                                         context.GetOptionalEndKey());

            if (mixedBuckets == null)
            {
                AddFallbackEdges(graph);
                mixedBuckets = GetPreferredMix(graph, optionalStartVertex,
                                               context.GetOptionalEndKey());
            }

            if (mixedBuckets == null)
            {
                mixedTracks = null;
                return false;
            }

            mixedTracks = UnpackBuckets(mixedBuckets);
            return true;
        }

        void AddPreferredEdges(IMutableEdgeListGraph<AutoMixingBucket, AutoMixEdge> graph)
        {
            AddEdges(graph, strategiesFactory.GetPreferredStrategiesInOrder());
            Log.DebugFormat("Added {0} preferred edges.", graph.EdgeCount);
        }

        void AddFallbackEdges(IMutableEdgeListGraph<AutoMixingBucket, AutoMixEdge> graph)
        {
            AddEdges(graph, strategiesFactory.GetNonPreferredCompatibleStrategies());
            Log.DebugFormat("Added {0} fallback edges (harmonic but not preferred).", graph.EdgeCount);
        }

        static AdjacencyGraph<AutoMixingBucket, AutoMixEdge> BuildGraph(
            IEnumerable<IMixItem> mixableTracks, 
            AutoMixingContext context, out AutoMixingBucket optionalStartVertex)
        {
            IList<AutoMixingBucket> buckets = mixableTracks
                .GroupBy(g => g.ActualKey)
                .Select(g => new AutoMixingBucket(g, g.Key))
                .ToList();

            Log.DebugFormat("{0} mixable tracks ({1} buckets aka vertices).",
                            mixableTracks.Count(), buckets.Count());

            var graph = new AdjacencyGraph<AutoMixingBucket, AutoMixEdge>();

            graph.AddVertexRange(buckets);

            // If we are auto-mixing a subset of tracks within a mix, we must
            // keep them harmonically compatible with the tracks immediately
            // before and after i.e. (preceeding)(tracks to mix)(following).
            //
            // To find such a path, we unfortunately can't just look for paths
            // where (start = preceeding and end = following) because they
            // might be using different keys that are not used by any of the
            // tracks in the graph.
            //
            // To get around this problem, we add empty 'placeholder' buckets
            // (vertices) to the graph for the preceeding/following keys, and
            // look for paths that use these vertices as start/end points.
            // This works because when we unpack the buckets, these vertices
            // simply unpack as empty (they don't contain any tracks) so we can
            // use them for the final path of mix items without producing any 
            // messy placeholder/temporary tracks.
            optionalStartVertex = AddPlaceholderVertexIfRequired(graph, context.GetOptionalStartKey());
            AddPlaceholderVertexIfRequired(graph, context.GetOptionalEndKey());

            return graph;
        }

        static AutoMixingBucket AddPlaceholderVertexIfRequired(
            AdjacencyGraph<AutoMixingBucket, AutoMixEdge> graph, HarmonicKey key)
        {
            if (key == null)
                return null;

            var vertex = new AutoMixingBucket(key);
            graph.AddVertex(vertex);
            return vertex;
        }

        static IEnumerable<IMixItem> UnpackBuckets(IEnumerable<AutoMixingBucket> mixedBuckets)
        {
            return mixedBuckets.SelectMany(b => b);
        }

        static IEnumerable<AutoMixingBucket> GetPreferredMix(
            IVertexListGraph<AutoMixingBucket, AutoMixEdge> graph,
            AutoMixingBucket optionalStartVertex,
            HarmonicKey optionalEndKey)
        {
            var algo = new AllLongestPathsAlgorithm<AutoMixingBucket, AutoMixEdge>(graph);

            HarmonicKey optionalStartKey = null;
            if (optionalStartVertex != null)
            {
                algo.SetRootVertex(optionalStartVertex);
                optionalStartKey = optionalStartVertex.ActualKey;
            }

            var stopwatch = Stopwatch.StartNew();
            algo.Compute();
            stopwatch.Stop();

            Log.DebugFormat("Found {0} paths in {1}.", algo.LongestPaths.Count(), stopwatch.Elapsed);

            if (!algo.LongestPaths.Any())
                return null;
            LogPaths(algo.LongestPaths);

            IEnumerable<AutoMixEdge> bestPath = GetPathSatisfyingOptionalStartAndEndKey(algo.LongestPaths,
                optionalStartKey, optionalEndKey);

            if (bestPath == null)
                return null;

            Log.DebugFormat("Using path: {0}", FormatPath(bestPath));

            return GetVertices(bestPath);
        }

        static IEnumerable<AutoMixEdge> GetPathSatisfyingOptionalStartAndEndKey(
            IEnumerable<IEnumerable<AutoMixEdge>> paths,
            HarmonicKey optionalStartKey,
            HarmonicKey optionalEndKey)
        {
            var validPaths = paths;

            if (optionalStartKey != null)
            {
                Log.DebugFormat("Required start key: {0}", optionalStartKey);
                validPaths = validPaths.Where(p => p.First().Source.ContainsKey(optionalStartKey));
            }

            if (optionalEndKey != null)
            {
                Log.DebugFormat("Required end key: {0}", optionalEndKey);
                validPaths = validPaths.Where(p => p.Last().Target.ContainsKey(optionalEndKey));
            }

            return validPaths
                .OrderBy(p => p.Sum(e => e.Cost))
                .FirstOrDefault();
        }

        static IEnumerable<AutoMixingBucket> GetVertices(IEnumerable<AutoMixEdge> path)
        {
            yield return path.First().Source;
            foreach (AutoMixEdge edge in path)
                yield return edge.Target;
        }

        /// <summary>
        /// Add any graph edge where there is any transition from one track to
        /// another, using one of the specified mixing strategies.
        /// </summary>
        void AddEdges(IMutableEdgeListGraph<AutoMixingBucket, AutoMixEdge> graph,
            IEnumerable<IMixingStrategy> strategies)
        {
            IEnumerable<AutoMixEdge> edges =
                from preceedingTrack in graph.Vertices
                from followingTrack in graph.Vertices
                where !followingTrack.Equals(preceedingTrack)
                from strategy in strategies
                where strategy.IsCompatible(preceedingTrack.ActualKey, followingTrack.ActualKey)
                orderby followingTrack.ActualKey, preceedingTrack.ActualKey
                let cost = edgeCostCalculator.CalculateCost(strategy)
                select new AutoMixEdge(preceedingTrack, followingTrack, strategy, cost);

            graph.AddEdgeRange(edges);
        }

        static void LogPaths(IEnumerable<IEnumerable<AutoMixEdge>> paths)
        {
            foreach (IEnumerable<AutoMixEdge> path in paths)
                Log.DebugFormat(FormatPath(path));
        }

        static string FormatPath(IEnumerable<AutoMixEdge> path)
        {
            IEnumerable<HarmonicKey> vertices = path
                .Select(e => e.Source.ActualKey)
                .Concat(new[] {path.Last().Target.ActualKey});

            return String.Format("{0} (cost: {1})", String.Join(" -> ", vertices), 
                path.Sum(e => e.Cost)); 
        }
    }
}