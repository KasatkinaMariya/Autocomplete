using System;
using System.Collections.Generic;

namespace Autocompletter
{
    public class SortedLinkedList<T>
        where T : IComparable<T>
    {
        private ListNode _head;
        private ListNode _last;
        private int _count;

        private readonly int _maxCapacity;

        public SortedLinkedList (int maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            ListNode curNode = _head;
            while (curNode != null)
            {
                yield return curNode.Value;
                curNode = curNode.Next;
            }
        }

        public void Add(T toAdd)
        {
            bool shouldBeAdded = (_count < _maxCapacity) || (_last.Value.CompareTo(toAdd) >= 1);
            if (shouldBeAdded)
            {
                DoAdd(toAdd);
                if (_count > _maxCapacity)
                    DoDeleteLast();
            }
        }

        private void DoDeleteLast()
        {
            _last = _last.Previous;
            _last.Next = null;
            _count--;
        }

        private void DoAdd(T valueToAdd)
        {
            if (_head == null)
                InsertAsHead(valueToAdd);
            else if (_head.Value.CompareTo(valueToAdd) >= 0) // _root.Value >= toAdd
                InsertBeforeHead(valueToAdd);
            else
            {
                var nodeToInsertAfter = FindMinGreaterNode(valueToAdd);

                if (nodeToInsertAfter.Next != null)
                    InsertInTheMiddle(valueToAdd,nodeToInsertAfter);
                else
                    InsertAsLast(valueToAdd);
            }

            _count++;
        }

        private ListNode FindMinGreaterNode(T value)
        {
            var minGreaterNode = _head;

            while (minGreaterNode.Next != null
                   && minGreaterNode.Next.Value.CompareTo(value) <= -1) // curNode.Next.Value < toAdd)
            {
                minGreaterNode = minGreaterNode.Next;
            }

            return minGreaterNode;
        }

        private void InsertAsHead(T valueToAdd)
        {
            _head = new ListNode(null, valueToAdd, null);
            _last = _head;
        }

        private void InsertBeforeHead(T valueToAdd)
        {
            var nodeToAdd = new ListNode(null, valueToAdd, _head);
            nodeToAdd.Next.Previous = nodeToAdd;
            _head = nodeToAdd;
        }

        private void InsertInTheMiddle(T valueToAdd, ListNode previousNode)
        {
            var nodeToAdd = new ListNode(previousNode, valueToAdd, previousNode.Next);
            nodeToAdd.Previous.Next = nodeToAdd;
            nodeToAdd.Next.Previous = nodeToAdd;
        }

        private void InsertAsLast(T valueToAdd)
        {
            var nodeToAdd = new ListNode(_last, valueToAdd, null);
            _last.Next = nodeToAdd;
            _last = nodeToAdd;
        }


        private class ListNode
        {
            public ListNode Next { get; set; }
            public ListNode Previous { get; set; }
            public T Value { get; private set; }

            public ListNode(ListNode previous, T value, ListNode next)
            {
                Next = next;
                Previous = previous;
                Value = value;
            }
        }
    }
}
