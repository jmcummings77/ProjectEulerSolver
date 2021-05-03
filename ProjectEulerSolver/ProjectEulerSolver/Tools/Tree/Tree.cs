using ProjectEulerSolver.Interfaces;
using System.Numerics;
using System;
using System.Collections.Generic;

namespace ProjectEulerSolver.Tools.Tree
{
    public class Tree
    {
        public Node Root { get; set; }
        

    }
    public class Node
    {
        public Node Parent { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node(int _Layer, int _Position, int _Value)
        {
            Layer = _Layer;
            Position = _Position;
            Value = _Value;
        }
        public int Layer { get; set; }
        public int Position { get; set; }
        public int Value { get; set; }
        public void AddChildren(Node FirstChild, Node SecondChild)
        {
            if(FirstChild.Value > SecondChild.Value)
            {
                Right = FirstChild;
                Left = SecondChild;
            }
            else
            {
                Right = SecondChild;
                Left = FirstChild;
            }
        }
    }
}