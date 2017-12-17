using EulerSolver.Interfaces;

namespace EulerSolver.Problems
{
    public abstract class BaseProblem : IProblem
    {
        public int Number  { get; set; }
        public string Prompt  { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Notes { get; set; }      
        public abstract void Solve();
    }
}