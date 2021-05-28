
using System;
using System.Collections.Generic;

namespace Heap Priority Queue
{
    public class PQ<T_elem, T_prio>
        where T_elem : IComparable
        where T_prio : IComparable
    {
        // Min heap satisfying heap property and sorted by priorities
        // Pop and MinPriority output the element with the lowest priority value
        public int Size { get; protected set; }
        public List<Item<T_elem, T_prio>>
        Heap
        { get; protected set; }

        public struct Item<T_e, T_p>
            where T_e : IComparable
            where T_p : IComparable
        {
            public T_e Element { get; set; }
            public T_p Priority { get; set; }
        }

        public PQ(params (T_elem ID, T_prio Priority)[] Items)
        {
            // Initialize size variable create and initialize list for heap
            Size = 0;
            Heap = new List<Item<T_elem, T_prio>>
            {
                // Add a buffer so all elements can be accessed by the parent-child relationship (divide by 2)
                // and so that Heap.Add() does not add to an empty first index
                new Item<T_elem, T_prio> { }
            };

            // Takes optional items in the constructor for nicer syntax
            for (int i = 0; i < Items.Length; i++)
            {
                Push(Items[i].ID, Items[i].Priority);
            }
        }

        #region Public Methods

        public void Push(T_elem elem, T_prio prio)
        { // Add an element with a priority to the priority queue
            Size++;

            Heap.Add(new Item<T_elem, T_prio>
            {
                Element = elem,
                Priority = prio
            });
            CascadeUp(Size);
        }

        public T_elem MinPriority()
        { // Outputs the element with lowest priority value
            return Size == 0 ? default : Heap[1].Element;
        }

        public T_elem Pop()
        { // Removes from the heap and outputs the element with lowest priority value
            if (Size == 0) { return default; }

            SwapNodes(1, Size--);
            CascadeDown();
            T_elem retval = Heap[Size + 1].Element;
            Heap.RemoveAt(Size + 1);
            return retval;
        }

        #endregion

        #region Hidden Methods
        private void CascadeUp(int curNode)
        { // Rearrange heap to move an item up to its proper position
            int curParent = curNode / 2;

            while (curParent >= 1 && Heap[curParent].Priority.CompareTo(Heap[curNode].Priority) > 0)
            // While the node is not the topmost node and the parent has a higher priority than the child
            {
                // Move the node up by swapping with its parent
                SwapNodes(curNode, curParent);
                curNode = curParent;
                curParent = curNode / 2;
            }
        }

        private void CascadeDown()
        { // Rearrange heap to move the item at the top of the heap to its proper location
            int curNode = 1;

            // assign childInd to the child of the root node with lower priority            
            int childInd = (3 > Size || Heap[2].Priority.CompareTo(Heap[3].Priority) <= 0) ? 2 : 3;

            while (childInd <= Size && Heap[childInd].Priority.CompareTo(Heap[curNode].Priority) < 0)
            // While the node's priority is larger than the child's
            {
                SwapNodes(curNode, childInd);
                curNode = childInd;

                childInd = (curNode * 2 + 1 > Size ||
                    Heap[curNode * 2].Priority.CompareTo(Heap[curNode * 2 + 1].Priority) <= 0
                    ) ? curNode * 2 : curNode * 2 + 1;
            }
        }

        private void SwapNodes(int node1, int node2)
        {
            Item<T_elem, T_prio> tempNode = Heap[node1];
            Heap[node1] = Heap[node2];
            Heap[node2] = tempNode;
        }
        #endregion
    }

}
