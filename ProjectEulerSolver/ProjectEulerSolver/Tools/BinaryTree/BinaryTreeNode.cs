using ProjectEulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace ProjectEulerSolver.Tools
{
    public class BinarySearchTreeNode<T> : Node<T>
    {
        public BinarySearchTreeNode() : base() {}
        public BinarySearchTreeNode(T data) : base(data, null) {}
        public BinarySearchTreeNode(T data, BinarySearchTreeNode<T> left, BinarySearchTreeNode<T> right)
        {
            base.Value = data;
            NodeList<T> children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;

            base.Neighbors = children;
        }

        public BinarySearchTreeNode<T> Left
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinarySearchTreeNode<T>) base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[0] = value;
            }
        }

        public BinarySearchTreeNode<T> Right
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinarySearchTreeNode<T>) base.Neighbors[1];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[1] = value;
            }
        }
        
    }
}