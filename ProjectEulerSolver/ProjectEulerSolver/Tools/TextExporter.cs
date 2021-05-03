using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEulerSolver.Tools
{ 
    public static class TextExporter
    {
        public static void Export(string filePath, Dictionary<int, string> output)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                foreach(int key in output.Keys)
                {
                    file.WriteLine(key.ToString() + "    " + output[key]);
                }
            }
        }
    }
}