using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProjectEulerSolver.ProjectEulerSolver.Tools
{
    public static class MiscTools
    {
        public static bool IsPythagoreanTriple(List<int> triple)
        {
            if (triple.Count() == 3)
            {
                triple.Sort();
                return (triple[0] * triple[0] + triple[1] * triple[1] == triple[2] * triple[2]);
            }
            return false;
        }
    }
}
