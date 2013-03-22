using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Services;

namespace MixPlanner.DomainModel
{
    /// <summary>
    /// Same as LongestPathAlgorithm, but doesn't stop after finding a path
    /// from the root vertex to all the other vertices - it keeps looking to
    /// find any others.
    /// </summary>
    public class AllLongestPathsAlgorithm<TVertex, TEdge> : LongestPathAlgorithm<TVertex, TEdge> 
        where TEdge : IEdge<TVertex>
    {
        public AllLongestPathsAlgorithm(IVertexListGraph<TVertex, TEdge> visitedGraph) 
            : base(visitedGraph)
        {
        }

        public AllLongestPathsAlgorithm(IAlgorithmComponent host, IVertexListGraph<TVertex, TEdge> visitedGraph) 
            : base(host, visitedGraph)
        {
        }

        protected override void InternalCompute()
        {
            // if there is a starting vertex, start whith him:
            TVertex rootVertex;
            if (TryGetRootVertex(out rootVertex))
                TryFindDeepest(rootVertex);
            else
            {
                var cancelManager = Services.CancelManager;
                // process each vertex 
                foreach (var u in VisitedGraph.Vertices)
                {
                    if (cancelManager.IsCancelling)
                        return;

                    IEnumerable<TEdge> path;
                    TryFindDeepest(u);
                }
            }
        }

        private void TryFindDeepest(TVertex root)
        {
            var stack = new Stack<TEdge>(VisitedGraph.VertexCount);
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(root);

            foreach (TEdge edge in outEdges)
            {
                FindDeepestRecursive(stack, edge);
            }
        }

        private void FindDeepestRecursive(Stack<TEdge> stack, TEdge root)
        {
            if (stack.Count == VisitedGraph.VertexCount - 2)
            {
                stack.Push(root);
                AddResult(stack.Reverse().ToList());
                return;
            }

            IEnumerable<TEdge> outEdges = VisitedGraph
                .OutEdges(root.Target)
                .Where(e => !StackContains(stack, e.Target))
                .ToList();

            if (!outEdges.Any())
                return;

            stack.Push(root);

            foreach (TEdge edge in outEdges)
                FindDeepestRecursive(stack, edge);

            stack.Pop();
        }
    }
}