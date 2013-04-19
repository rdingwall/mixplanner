using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Services;

namespace MixPlanner.DomainModel.AutoMixing
{
    /// <summary>
    /// Modified depth-first search (DFS) algorithm for finding a path from
    /// one vertex to all other vertices, without visiting any vertices twice.
    /// It works by traversing unvisited vertices using DFS, until it has
    /// reached a depth n (where n is the number of vertices in the graph).
    /// At this point it is complete, and returns the path. While traversing,
    /// it keeps track of visited vertices, and will not follow any cycles.
    /// 
    /// This is designed to find a DJ mix containing all the requested tracks
    /// (vertexes), using only preferred harmonic mixing strategy transitions
    /// between them (edges).
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
    public class LongestPathAlgorithm<TVertex, TEdge> :
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
        
        protected void AddResult(IEnumerable<TEdge> path)
        {
            longestPaths.Add(path);
        }

        protected override void InternalCompute()
        {
            // if there is a starting vertex, start whith him:
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
            {
                IEnumerable<TEdge> path;
                if (TryFindDeepest(rootVertex, out path))
                    AddResult(path);
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
                        AddResult(path);
                }
            }
        }

        private bool TryFindDeepest(TVertex root, out IEnumerable<TEdge> path)
        {
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(root);

            foreach (TEdge edge in outEdges)
            {
                var stack = new SuperStack<TVertex, TEdge>(VisitedGraph.VertexCount);

                if (FindDeepestRecursive(stack, edge))
                {
                    path = stack.Reverse().ToList();
                    return true;
                }
            }

            path = null;
            return false;
        }

        protected static bool StackContains(SuperStack<TVertex, TEdge> stack, TVertex vertex)
        {
            if (stack.IsEmpty())
                return false;

            TEdge bottomOfStack;
            if (stack.TryPeekBottom(out bottomOfStack) && bottomOfStack.Source.Equals(vertex))
                return true;

            return stack.ContainsVertex(vertex);
        }

        private bool FindDeepestRecursive(SuperStack<TVertex, TEdge> stack, TEdge root)
        {
            // Stop if we've already visted all the nodes (remember edge count
            // will always be lower than vertex count).
            if (stack.Count == VisitedGraph.VertexCount - 2)
            {
                stack.Push(root);
                return true;
            }

            IEnumerable<TEdge> outEdges = VisitedGraph
                .OutEdges(root.Target)
                .Where(e => !StackContains(stack, e.Target))
                // Need to check we aren't looping back to the root cos on the
                // first pass the root won't be in the stack yet.
                .Where(e => !root.Source.Equals(e.Target))
                .ToList();

            if (!outEdges.Any())
                return false;

            if (root.Source.Equals(HarmonicKey.Key7A) || root.Target.Equals(HarmonicKey.Key7A))
                Debugger.Break();

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
