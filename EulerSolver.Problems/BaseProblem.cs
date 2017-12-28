using EulerSolver.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EulerSolver.Problems
{
    public abstract class BaseProblem : IProblem
    {
        public int Number  { get; set; }
        public string Prompt  { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Notes { get; set; }    
        public string LogFilePath { get; set; }
        public abstract void Solve();
        public List<string> LogList { get; set; }
        public void LogToFile()
        {
            if (LogFilePath != "")
            {
                if(LogList != null)
                {
                    if(LogList.Any())
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFilePath))
                        {
                            foreach (string item in LogList)
                            {
                                file.WriteLine(item);
                            }
                        }
                    }
                }
            }
        }
        public void LogToFile(string LineToLog)
        {
            if(LogFilePath != "")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFilePath))
                {
                  file.WriteLine(LineToLog);
                }
            }

        }
        public void LogToFile<T>(List<T> ListToLog)
        {
            if (LogFilePath != "")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFilePath))
                {
                    foreach(var item in ListToLog)
                    {
                        file.WriteLine(item.ToString());
                    }
                }
            }
        }
    }
}