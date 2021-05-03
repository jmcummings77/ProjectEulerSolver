using ProjectEulerSolver.Interfaces;
using ProjectEulerSolver.Tools;
using ProjectEulerSolver.Model;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace ProjectEulerSolver.Problems
{
    public class Problem79 : BaseProblem, IProblem
    {
        public Problem79()
        {
            Number = 79;
            Prompt = "A common security method used for online banking is to ask the user for three random characters from a passcode. For example, if the passcode was 531278, they may ask for the 2nd, 3rd, and 5th characters; the expected reply would be: 317. " +
                     "The text file, keylog.txt, contains fifty successful login attempts. " +
                     "Given that the three characters are always asked for in order, analyse the file so as to determine the shortest possible secret passcode of unknown length.";
        }
        public override void Solve()
        {
            ReadLinesFromFile();
            keys = keys.Distinct().ToList();


        }
        private List<string> keys = new List<string>();
        private void ReadLinesFromFile()
        {
            string SourceFile = @"C:\Users\us48610\Desktop\log.txt";
            string line;
            using (System.IO.StreamReader file = new System.IO.StreamReader(SourceFile))
            {
                while ((line = file.ReadLine()) != null)
                {
                    keys.Add(line);
                }
            }
        }
    }
}
