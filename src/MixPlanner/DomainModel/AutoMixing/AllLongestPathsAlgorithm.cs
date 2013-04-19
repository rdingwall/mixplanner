using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Services;

namespace MixPlanner.DomainModel.AutoMixing
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

                    TryFindDeepest(u);
                }
            }
        }

        private void TryFindDeepest(TVertex root)
        {
            var stack = new SuperStack<TVertex, TEdge>(VisitedGraph.VertexCount);
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(root);

            foreach (TEdge edge in outEdges)
            {
                FindDeepestRecursive(stack, edge);
            }
        }

        private void FindDeepestRecursive(SuperStack<TVertex, TEdge> stack, TEdge root)
        {
            // Stop if we've already visted all the nodes (remember edge count
            // will always be lower than vertex count).
            if (stack.Count == VisitedGraph.VertexCount - 2)
            {
                var result = stack.Reverse().ToList();
                result.Add(root);
                AddResult(result);
                return;
            }

            IEnumerable<TEdge> outEdges = VisitedGraph
                .OutEdges(root.Target)
                .Where(e => !StackContains(stack, e.Target))
                // Need to check we aren't looping back to the root cos on the
                // first pass the root won't be in the stack yet.
                .Where(e => !root.Source.Equals(e.Target))
                .ToList();

            if (!outEdges.Any())
                return;

            if (root.Source.Equals(HarmonicKey.Key7A) || root.Target.Equals(HarmonicKey.Key7A))
                Debugger.Break();

            stack.Push(root);

            foreach (TEdge edge in outEdges)
                FindDeepestRecursive(stack, edge);

            stack.Pop();
        }
    }
}