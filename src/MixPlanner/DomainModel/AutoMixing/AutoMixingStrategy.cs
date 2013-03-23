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

            IEnumerable<AutoMixingBucket> buckets = mixableTracks
                .GroupBy(g => g.ActualKey)
                .Select(g => new AutoMixingBucket(g, g.Key))
                .ToList();

            Log.DebugFormat("{0} mixable tracks ({1} buckets), {2} unmixable tracks (unknown key/BPM).",
                mixableTracks.Count(), buckets.Count(), unknownTracks.Count());

            var graph = new AdjacencyGraph<AutoMixingBucket, AutoMixEdge>();

            graph.AddVertexRange(buckets);
            AddEdges(graph, strategiesFactory.GetPreferredStrategiesInOrder(), buckets);
            Log.DebugFormat("Added {0} preferred edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);

            IEnumerable<AutoMixingBucket> mixedBuckets = GetPreferredMix(graph, context);

            if (mixedBuckets == null)
            {
                Log.Debug("Failed to find any auto mix paths. Returning unmodified tracks.");
                return AutoMixingResult.Failure(context);
            }

            IEnumerable<IMixItem> mixedTracks = UnpackBuckets(mixedBuckets);

            return AutoMixingResult.Success(context, mixedTracks, unknownTracks);
        }

        static IEnumerable<IMixItem> UnpackBuckets(IEnumerable<AutoMixingBucket> mixedBuckets)
        {
            return mixedBuckets.SelectMany(b => b);
        }

        static IEnumerable<AutoMixingBucket> GetPreferredMix(
            IVertexListGraph<AutoMixingBucket, AutoMixEdge> graph, 
            AutoMixingContext context)
        {
            var algo = new LongestPathAlgorithm<AutoMixingBucket, AutoMixEdge>(graph);

            var stopwatch = Stopwatch.StartNew();
            algo.Compute();
            stopwatch.Stop();

            Log.DebugFormat("Found {0} paths in {1}.", algo.LongestPaths.Count(), stopwatch.Elapsed);

            if (!algo.LongestPaths.Any())
                return null;
            LogPaths(algo.LongestPaths);

            IEnumerable<AutoMixEdge> bestPath = GetBestPath(algo.LongestPaths, context);

            if (bestPath == null)
                return null;

            Log.DebugFormat("Using path: {0}", FormatPath(bestPath));

            return GetVertices(bestPath);
        }

        static IEnumerable<AutoMixEdge> GetBestPath(
            IEnumerable<IEnumerable<AutoMixEdge>> paths,
            AutoMixingContext context)
        {
            var validPaths = paths;

            if (context.PreceedingTrack != null)
            {
                Log.DebugFormat("Required preceeding track: {0}", context.PreceedingTrack.ActualKey);
                validPaths = validPaths.Where(p => p.First().Source.Equals(context.PreceedingTrack));
            }
            if (context.FollowingTrack != null)
            {
                Log.DebugFormat("Required following track: {0}", context.FollowingTrack.ActualKey);
                validPaths = validPaths.Where(p => p.Last().Target.Equals(context.FollowingTrack));
            }

            return validPaths.FirstOrDefault();
        }

        static IEnumerable<AutoMixingBucket> GetVertices(IEnumerable<AutoMixEdge> path)
        {
            yield return path.First().Source;
            foreach (var edge in path)
                yield return edge.Target;
        }

        /// <summary>
        /// Add any graph edge where there is any transition from one track to
        /// another, using one of the specified mixing strategies.
        /// </summary>
        static void AddEdges(IMutableEdgeListGraph<AutoMixingBucket, AutoMixEdge> graph,
            IEnumerable<IMixingStrategy> strategies,
            IEnumerable<AutoMixingBucket> buckets)
        {
            IEnumerable<AutoMixEdge> edges =
                from preceedingTrack in buckets
                from followingTrack in buckets
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
            var vertices = path.Select(e => e.Source.ActualKey)
                               .Concat(new[] {path.Last().Target.ActualKey});
            return String.Join(" -> ", vertices);
        }
    }
}