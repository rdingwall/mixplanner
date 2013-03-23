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

    public abstract class AutoMixingStrategy {}

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

            var unknownTracks = context.TracksToMix.Where(c => c.IsUnknownKeyOrBpm).ToList();
            var mixableTracks = context.TracksToMix.Except(unknownTracks);

            var graph = new AdjacencyGraph<T, AutoMixEdge<T>>();

            graph.AddVertexRange(mixableTracks);
            AddEdges(graph, strategiesFactory.GetPreferredStrategiesInOrder(), mixableTracks);
            log.DebugFormat("Added {0} preferred edges, {1} vertices.", graph.EdgeCount, graph.VertexCount);

            IEnumerable<T> mixedTracks = GetPreferredMix(graph, context);

            if (mixedTracks == null)
                return new AutoMixingResult<T>(context.TracksToMix, context);

            return new AutoMixingResult<T>(mixedTracks, context, unknownTracks);
        }

        static IEnumerable<T> GetPreferredMix(
            IVertexListGraph<T, AutoMixEdge<T>> graph, 
            AutoMixingContext<T> context)
        {
            var algo = new LongestPathAlgorithm<T, AutoMixEdge<T>>(graph);

            var stopwatch = Stopwatch.StartNew();
            algo.Compute();
            stopwatch.Stop();

            log.DebugFormat("Found {0} paths in {1}.", algo.LongestPaths.Count(), stopwatch.Elapsed);

            if (!algo.LongestPaths.Any())
                return null;
            LogPaths(algo.LongestPaths);

            IEnumerable<AutoMixEdge<T>> bestPath = GetBestPath(algo.LongestPaths, context);

            log.DebugFormat("Using path: {0}", FormatPath(bestPath));

            return GetVertices(bestPath);
        }

        static IEnumerable<AutoMixEdge<T>> GetBestPath(
            IEnumerable<IEnumerable<AutoMixEdge<T>>> paths,
            AutoMixingContext<T> context)
        {
            var validPaths = paths;

            if (context.PreceedingTrack != null)
                validPaths = validPaths.Where(p => p.First().Source.Equals(context.PreceedingTrack));

            if (context.FollowingTrack != null)
                validPaths = validPaths.Where(p => p.Last().Target.Equals(context.FollowingTrack));

            return validPaths.FirstOrDefault();
        }

        static IEnumerable<T> GetVertices(IEnumerable<AutoMixEdge<T>> path)
        {
            yield return path.First().Source;
            foreach (var edge in path)
                yield return edge.Target;
        }

        /// <summary>
        /// Add any graph edge where there is any transition from one track to
        /// another, using one of the specified mixing strategies.
        /// </summary>
        static void AddEdges(IMutableEdgeListGraph<T, AutoMixEdge<T>> graph,
            IEnumerable<IMixingStrategy> strategies,
            IEnumerable<T> tracks)
        {
            IEnumerable<AutoMixEdge<T>> edges =
                from preceedingTrack in tracks
                from followingTrack in tracks
                where !followingTrack.Equals(preceedingTrack)
                from strategy in strategies
                where strategy.IsCompatible(preceedingTrack.PlaybackSpeed, followingTrack.PlaybackSpeed)
                orderby followingTrack.PlaybackSpeed.ActualKey, preceedingTrack.PlaybackSpeed.ActualKey
                select new AutoMixEdge<T>(preceedingTrack, followingTrack, strategy);

            graph.AddEdgeRange(edges);
        }

        static void LogPaths(IEnumerable<IEnumerable<AutoMixEdge<T>>> paths)
        {
            foreach (IEnumerable<AutoMixEdge<T>> path in paths)
                log.DebugFormat(FormatPath(path));
        }

        static string FormatPath(IEnumerable<AutoMixEdge<T>> path)
        {
            var vertices = path.Select(e => e.Source.PlaybackSpeed.ActualKey)
                               .Concat(new[] {path.Last().Target.PlaybackSpeed.ActualKey});
            return String.Join(" -> ", vertices);
        }
    }
}