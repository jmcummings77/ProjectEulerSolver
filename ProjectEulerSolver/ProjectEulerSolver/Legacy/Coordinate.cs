using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Legacy
{
    public class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}