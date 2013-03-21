using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Services;

namespace MixPlanner.DomainModel
{
    /// <summary>
    /// Modified depth-first search (DFS) algorithm for finding a path from
    /// one vertex to all other vertices, without visiting any vertices twice.
    /// Designed to find a DJ mix containing all the requested tracks
    /// (vertexes), using only preferred harmonic mixing strategies (edges).
    /// 
    /// If more than one path (mix) exists, only the first is returned.
    /// 
    /// We are not interested in incomplete paths (mixes that miss some tracks
    /// out), so if a path is found, but doesn't quite reach all the vertices,
    /// it will not be returned.
    /// 
    /// This algorithm assumes a directed (a)cyclic graph with a small number
    /// of vertices and edges - e.g. 12 harmonic keys x 2 scales with 5 mixing
    /// strategies gives us a nice manageable ceiling in any mix of 24 vertices
    /// and 96 edges.
    /// </summary>
    public sealed class LongestPathAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex, 
        IVertexListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        private readonly IList<IEnumerable<TEdge>> longestPaths;

        public LongestPathAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(null, visitedGraph)
        { }

        public LongestPathAlgorithm(
            IAlgorithmComponent host,
            IVertexListGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
            longestPaths = new List<IEnumerable<TEdge>>();
        }

        public IEnumerable<IEnumerable<TEdge>> LongestPaths { get { return longestPaths; } } 

        protected override void InternalCompute()
        {
            // if there is a starting vertex, start whith him:
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
            {
                IEnumerable<TEdge> path;
                if (TryFindDeepest(rootVertex, out path))
                    longestPaths.Add(path);
            }
            else
            {
                var cancelManager = Services.CancelManager;
                // process each vertex 
                foreach (var u in VisitedGraph.Vertices)
                {
                    if (cancelManager.IsCancelling)
                        return;

                    IEnumerable<TEdge> path;
                    if (TryFindDeepest(u, out path))
                        longestPaths.Add(path);
                }
            }
        }

        private bool TryFindDeepest(TVertex root, out IEnumerable<TEdge> path)
        {
            var stack = new Stack<TEdge>(VisitedGraph.VertexCount);
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(root);

            foreach (TEdge edge in outEdges)
            {
                if (FindDeepestRecursive(stack, edge))
                {
                    path = stack.Reverse().ToList();
                    return true;
                }
            }

            path = null;
            return false;
        }

        private static bool StackContains(Stack<TEdge> stack, TVertex vertex)
        {
            if (!stack.Any())
                return false;

            var bottomOfStack = stack.Last();
            return bottomOfStack.Source.Equals(vertex) ||
                   stack.Any(e => e.Target.Equals(vertex));
        }

        private bool FindDeepestRecursive(Stack<TEdge> stack, TEdge root)
        {
            if (stack.Count == VisitedGraph.VertexCount - 2)
            {
                stack.Push(root);
                return true;
            }

            IEnumerable<TEdge> outEdges = VisitedGraph
                .OutEdges(root.Target)
                .Where(e => !StackContains(stack, e.Target))
                .ToList();

            if (!outEdges.Any())
                return false;

            stack.Push(root);

            foreach (TEdge edge in outEdges)
            {
                if (FindDeepestRecursive(stack, edge))
                    return true;
            }

            stack.Pop();

            return false;
        }
    }
}
