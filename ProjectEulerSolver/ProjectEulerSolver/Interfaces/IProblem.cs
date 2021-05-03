namespace ProjectEulerSolver.Interfaces
{
    public interface IProblem
    {
        int Number  { get; set; }
        string Prompt  { get; set; }
        string Input { get; set; }
        string Output { get; set; }
        string Notes { get; set; }
    }
}