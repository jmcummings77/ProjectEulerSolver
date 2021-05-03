using ProjectEulerSolver.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectEulerSolver.Problems
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
            if(LogFilePath == null)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                LogFilePath = Path.Combine(path, @"Problem" + Number.ToString() + ".txt");
            }
            if (LogFilePath == "")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                LogFilePath = Path.Combine(path, @"Problem" + Number.ToString() + ".txt");
            }
            if(LogList != null)
            {
                if(LogList.Any())
                {
                    if (!File.Exists(LogFilePath))
                    {
                        using (System.IO.StreamWriter file = File.CreateText(LogFilePath))
                        {
                            foreach (string item in LogList)
                            {
                                file.WriteLine(item);
                            }
                        }
                    }
                    else
                    {
                        using (System.IO.StreamWriter file = File.AppendText(LogFilePath))
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
            if (LogFilePath == null)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                LogFilePath = Path.Combine(path, @"Problem" + Number.ToString() + ".txt");
            }
            if (LogFilePath == "")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                LogFilePath = Path.Combine(path, @"Problem" + Number.ToString() + ".txt");
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFilePath))
            {
                file.WriteLine(LineToLog);
            }
        }
        public void LogToFile<T>(List<T> ListToLog)
        {
            if (LogFilePath == null)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                LogFilePath = Path.Combine(path, @"Problem" + Number.ToString() + ".txt");
            }
            if (LogFilePath == "")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                LogFilePath = Path.Combine(path, @"Problem" + Number.ToString() + ".txt");
            }
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