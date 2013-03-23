using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QuickGraph;
using log4net;

namespace MixPlanner.DomainModel.AutoMixing
{
    public interface IAutoMixingStrategy<T> where T : IAutoMixable
    {
        AutoMixingResult<T> AutoMix(AutoMixingContext<T> context);
    }

    public abstract class AutoMixingStrategy {} // just for Logger name

    public class AutoMixingStrategy<T> : AutoMixingStrategy, IAutoMixingStrategy<T> 
        where T : IAutoMixable
    {
        static readonly ILog log = LogManager.GetLogger(typeof (AutoMixingStrategy)); 
        readonly IMixingStrategiesFactory strategiesFactory;

        public AutoMixingStrategy(IMixingStrategiesFactory strategiesFactory)
        {
            if (strategiesFactory == null) throw new ArgumentNullException("strategiesFactory");
            this.strategiesFactory = strategiesFactory;
        }

        public AutoMixingResult<T> AutoMix(AutoMixingContext<T> context)
        {
            if (context == null) throw new ArgumentNullException("context");

            IEnumerable<T> unknownTracks = context.TracksToMix.Where(c => c.IsUnknownKeyOrBpm).ToList();
            IEnumerable<T> mixableTracks = context.TracksToMix.Except(unknownTracks);

            IEnumerable<AutoMixingBucket<T>> buckets = mixableTracks
                .GroupBy(g => g.ActualKey)
                .Select(g => new AutoMixingBucket<T>(g, g.Key))
                .ToList();

            log.DebugFormat("{0} mixable tracks ({1} buckets), {2} unmixable tracks (unknown key/BPM).",
                mixableTracks.Count(), buckets.Count(), unknownTracks.Count());

            var graph = new AdjacencyGraph<AutoMixingBucket<T>, AutoMixEdge<AutoMixingBucket<T>>>();

            graph.AddVertexRange(buckets);
            AddEdges(graph, strategiesFactory.GetPreferredStrategiesInOrder(), buckets);
            log.DebugFormat("Added {0} preferred edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);

            IEnumerable<AutoMixingBucket<T>> mixedBuckets = GetPreferredMix(graph, context);

            if (mixedBuckets == null)
            {
                log.Debug("Failed to find any auto mix paths. Returning unmodified tracks.");
                return AutoMixingResult.Failure(context);
            }

            IEnumerable<T> mixedTracks = UnpackBuckets(mixedBuckets);

            return AutoMixingResult.Success(context, mixedTracks, unknownTracks);
        }

        static IEnumerable<T> UnpackBuckets(IEnumerable<AutoMixingBucket<T>> mixedBuckets)
        {
            return mixedBuckets.SelectMany(b => b);
        }

        static IEnumerable<AutoMixingBucket<T>> GetPreferredMix(
            IVertexListGraph<AutoMixingBucket<T>, AutoMixEdge<AutoMixingBucket<T>>> graph, 
            AutoMixingContext<T> context)
        {
            var algo = new LongestPathAlgorithm<AutoMixingBucket<T>, AutoMixEdge<AutoMixingBucket<T>>>(graph);

            var stopwatch = Stopwatch.StartNew();
            algo.Compute();
            stopwatch.Stop();

            log.DebugFormat("Found {0} paths in {1}.", algo.LongestPaths.Count(), stopwatch.Elapsed);

            if (!algo.LongestPaths.Any())
                return null;
            LogPaths(algo.LongestPaths);

            IEnumerable<AutoMixEdge<AutoMixingBucket<T>>> bestPath = GetBestPath(algo.LongestPaths, context);

            if (bestPath == null)
                return null;

            log.DebugFormat("Using path: {0}", FormatPath(bestPath));

            return GetVertices(bestPath);
        }

        static IEnumerable<AutoMixEdge<AutoMixingBucket<T>>> GetBestPath(
            IEnumerable<IEnumerable<AutoMixEdge<AutoMixingBucket<T>>>> paths,
            AutoMixingContext<T> context)
        {
            var validPaths = paths;

            if (context.PreceedingTrack != null)
            {
                log.DebugFormat("Required preceeding track: {0}", context.PreceedingTrack.ActualKey);
                validPaths = validPaths.Where(p => p.First().Source.Equals(context.PreceedingTrack));
            }
            if (context.FollowingTrack != null)
            {
                log.DebugFormat("Required following track: {0}", context.FollowingTrack.ActualKey);
                validPaths = validPaths.Where(p => p.Last().Target.Equals(context.FollowingTrack));
            }

            return validPaths.FirstOrDefault();
        }

        static IEnumerable<AutoMixingBucket<T>> GetVertices(IEnumerable<AutoMixEdge<AutoMixingBucket<T>>> path)
        {
            yield return path.First().Source;
            foreach (var edge in path)
                yield return edge.Target;
        }

        /// <summary>
        /// Add any graph edge where there is any transition from one track to
        /// another, using one of the specified mixing strategies.
        /// </summary>
        static void AddEdges(IMutableEdgeListGraph<AutoMixingBucket<T>, AutoMixEdge<AutoMixingBucket<T>>> graph,
            IEnumerable<IMixingStrategy> strategies,
            IEnumerable<AutoMixingBucket<T>> buckets)
        {
            IEnumerable<AutoMixEdge<AutoMixingBucket<T>>> edges =
                from preceedingTrack in buckets
                from followingTrack in buckets
                where !followingTrack.Equals(preceedingTrack)
                from strategy in strategies
                where strategy.IsCompatible(preceedingTrack.ActualKey, followingTrack.ActualKey)
                orderby followingTrack.ActualKey, preceedingTrack.ActualKey
                select new AutoMixEdge<AutoMixingBucket<T>>(preceedingTrack, followingTrack, strategy);

            graph.AddEdgeRange(edges);
        }

        static void LogPaths(IEnumerable<IEnumerable<AutoMixEdge<AutoMixingBucket<T>>>> paths)
        {
            foreach (IEnumerable<AutoMixEdge<AutoMixingBucket<T>>> path in paths)
                log.DebugFormat(FormatPath(path));
        }

        static string FormatPath(IEnumerable<AutoMixEdge<AutoMixingBucket<T>>> path)
        {
            var vertices = path.Select(e => e.Source.ActualKey)
                               .Concat(new[] {path.Last().Target.ActualKey});
            return String.Join(" -> ", vertices);
        }
    }
}