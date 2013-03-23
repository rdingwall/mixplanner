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

        public AutoMixingStrategy(IMixingStrategiesFactory strategiesFactory)
        {
            if (strategiesFactory == null) throw new ArgumentNullException("strategiesFactory");
            this.strategiesFactory = strategiesFactory;
        }

        public AutoMixingResult AutoMix(AutoMixingContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            IEnumerable<IMixItem> unknownTracks = context.TracksToMix.Where(c => c.IsUnknownKeyOrBpm).ToList();
            IEnumerable<IMixItem> mixableTracks = context.TracksToMix.Except(unknownTracks);

            AdjacencyGraph<AutoMixingBucket, AutoMixEdge> graph 
                = BuildGraph(mixableTracks, unknownTracks, context);

            AddEdges(graph, strategiesFactory.GetPreferredStrategiesInOrder());
            Log.DebugFormat("Added {0} preferred edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);

            IEnumerable<AutoMixingBucket> mixedBuckets = GetPreferredMix(graph, 
                context.GetOptionalStartKey(), context.GetOptionalEndKey());

            if (mixedBuckets == null)
            {
                Log.Debug("Failed to find any auto mix paths. Returning unmodified tracks.");
                return AutoMixingResult.Failure(context);
            }

            IEnumerable<IMixItem> mixedTracks = UnpackBuckets(mixedBuckets);

            return AutoMixingResult.Success(context, mixedTracks, unknownTracks);
        }

        static AdjacencyGraph<AutoMixingBucket, AutoMixEdge> BuildGraph(
            IEnumerable<IMixItem> mixableTracks, 
            IEnumerable<IMixItem> unknownTracks, 
            AutoMixingContext context)
        {
            IList<AutoMixingBucket> buckets = mixableTracks
                .GroupBy(g => g.ActualKey)
                .Select(g => new AutoMixingBucket(g, g.Key))
                .ToList();

            Log.DebugFormat("{0} mixable tracks ({1} buckets), {2} unmixable tracks (unknown key/BPM).",
                            mixableTracks.Count(), buckets.Count(), unknownTracks.Count());

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
            AddPlaceholderVertexIfRequired(graph, context.GetOptionalStartKey());
            AddPlaceholderVertexIfRequired(graph, context.GetOptionalEndKey());

            return graph;
        }

        static void AddPlaceholderVertexIfRequired(
            AdjacencyGraph<AutoMixingBucket, AutoMixEdge> graph, HarmonicKey key)
        {
            if (key == null)
                return;

            if (graph.Vertices.Any(v => v.ContainsKey(key)))
                return;

            graph.AddVertex(new AutoMixingBucket(key));
        }

        static IEnumerable<IMixItem> UnpackBuckets(IEnumerable<AutoMixingBucket> mixedBuckets)
        {
            return mixedBuckets.SelectMany(b => b);
        }

        static IEnumerable<AutoMixingBucket> GetPreferredMix(
            IVertexListGraph<AutoMixingBucket, AutoMixEdge> graph,
            HarmonicKey optionalStartKey,
            HarmonicKey optionalEndKey)
        {
            var algo = new AllLongestPathsAlgorithm<AutoMixingBucket, AutoMixEdge>(graph);

            var stopwatch = Stopwatch.StartNew();
            algo.Compute();
            stopwatch.Stop();

            Log.DebugFormat("Found {0} paths in {1}.", algo.LongestPaths.Count(), stopwatch.Elapsed);

            if (!algo.LongestPaths.Any())
                return null;
            LogPaths(algo.LongestPaths);

            IEnumerable<AutoMixEdge> bestPath = GetBestPath(algo.LongestPaths, 
                optionalStartKey, optionalEndKey);

            if (bestPath == null)
                return null;

            Log.DebugFormat("Using path: {0}", FormatPath(bestPath));

            return GetVertices(bestPath);
        }

        static IEnumerable<AutoMixEdge> GetBestPath(
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

            return validPaths.FirstOrDefault();
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
        static void AddEdges(IMutableEdgeListGraph<AutoMixingBucket, AutoMixEdge> graph,
            IEnumerable<IMixingStrategy> strategies)
        {
            IEnumerable<AutoMixEdge> edges =
                from preceedingTrack in graph.Vertices
                from followingTrack in graph.Vertices
                where !followingTrack.Equals(preceedingTrack)
                from strategy in strategies
                where strategy.IsCompatible(preceedingTrack.ActualKey, followingTrack.ActualKey)
                orderby followingTrack.ActualKey, preceedingTrack.ActualKey
                select new AutoMixEdge(preceedingTrack, followingTrack, strategy);

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

            return String.Join(" -> ", vertices);
        }
    }
}