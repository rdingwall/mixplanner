using System;
using System.Collections;
using System.Collections.Generic;
using QuickGraph;

namespace MixPlanner.DomainModel.AutoMixing
{
    /// <summary>
    /// Hacky implementation of a stack with O(1) lookup for bottom of stack
    /// and a HashSet for lookup if it contains existing vertices.
    /// </summary>
    public class SuperStack<TVertex, TEdge> : IEnumerable<TEdge>
        where TEdge : IEdge<TVertex>
    {
        readonly Stack<TEdge> stack;
        readonly HashSet<TVertex> visitedTargetVertices;
        TEdge bottom;

        public SuperStack(int initialCapacity)
        {
            stack = new Stack<TEdge>(initialCapacity);
            visitedTargetVertices = new HashSet<TVertex>();
        }

        public int Count
        {
            get { return stack.Count; }
        }

        public bool IsEmpty()
        {
            return stack.Count == 0;
        }

        public void Push(TEdge item)
        {
            if (stack.Count == 0)
                bottom = item;
           
            visitedTargetVertices.Add(item.Target);

            stack.Push(item);
        }

        public void Pop()
        {
            var item = stack.Pop();
            visitedTargetVertices.Remove(item.Target);

            if (IsEmpty())
                bottom = default(TEdge);
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return visitedTargetVertices.Contains(vertex);
        }

        public bool TryPeekBottom(out TEdge bottom)
        {
            if (IsEmpty())
            {
                bottom = default(TEdge);
                return false;
            }

            bottom = this.bottom;
            return true;
        }

        public IEnumerator<TEdge> GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}