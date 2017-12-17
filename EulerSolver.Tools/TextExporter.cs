using System;
using System.Collections.Generic;
using System.Linq;

namespace EulerSolver.Tools
{
/// Summary
/// Class to hold the code for the first fifteen Euler solutions before I started
/// trying to keep the code base organized   
    public static class TextExporter
    {
        public static void Export(string filePath, Dictionary<int, string> output)
        {
            using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(filePath))
            {
                foreach(int key in output.Keys)
                {
                    file.WriteLine(key.ToString() + "    " + output[key]);
                }
            }
        }
    }
}