namespace ProjectEulerSolver;

/// <summary>Discovers every concrete <see cref="Problem"/> in this assembly.</summary>
public static class ProblemCatalog
{
    private static readonly Lazy<IReadOnlyList<Problem>> Problems = new(Discover);

    /// <summary>All problems, ordered by number.</summary>
    public static IReadOnlyList<Problem> All => Problems.Value;

    /// <summary>Returns the problem with the given number, or <c>null</c> if it has not been solved.</summary>
    public static Problem? Find(int number) => All.FirstOrDefault(p => p.Number == number);

    private static IReadOnlyList<Problem> Discover() =>
        typeof(Problem).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Problem)))
            .Select(t => (Problem)Activator.CreateInstance(t)!)
            .OrderBy(p => p.Number)
            .ToArray();
}
