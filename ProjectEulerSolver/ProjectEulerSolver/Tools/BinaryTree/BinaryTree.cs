using ProjectEulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace ProjectEulerSolver.Tools
{
    public class BinarySearchTree<T>
    {
        private BinarySearchTreeNode<T> root;
        public BinarySearchTree()
        {
            root = null;
        }
        public virtual void Clear()
        {
            root = null;
        }
        public BinarySearchTreeNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
        public bool Contains(T data)
        {
            BinarySearchTreeNode<T> current = root;
            int result;
            while (current != null)
            {
                result = comparer.Compare(current.Value, data);
                if (result == 0)
                    return true;
                else if (result > 0)
                    current = current.Left;
                else if (result < 0)
                    current = current.Right;
            }

            return false;       
        }
        private IComparer<T> comparer = Comparer<T>.Default;
        public int Count { get; set; }
        public virtual void Add(T data)
        {
            BinarySearchTreeNode<T> n = new BinarySearchTreeNode<T>(data);
            int result;

            BinarySearchTreeNode<T> current = root, parent = null;
            while (current != null)
            {
                result = comparer.Compare(current.Value, data);
                if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
            }

            Count++;
            if (parent == null)
                root = n;
            else
            {
                result = comparer.Compare(parent.Value, data);
                if (result > 0)
                    parent.Left = n;
                else
                    parent.Right = n;
            }
        }
    }
}