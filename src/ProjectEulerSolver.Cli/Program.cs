using System.Diagnostics;

namespace ProjectEulerSolver.Cli;

/// <summary>Command-line runner: solves one, several or all problems and reports timings.</summary>
public static class Program
{
    public static int Main(string[] args) =>
        args switch
        {
            [] or ["--help"] or ["-h"] => Usage(),
            ["--all"] => Run(ProblemCatalog.All),
            ["--list"] => List(),
            _ => RunSelected(args),
        };

    private static int Usage()
    {
        Console.WriteLine("""
            Project Euler solver

            usage:
              euler <number> [<number> ...]   solve the given problem(s)
              euler --all                     solve every problem and report timings
              euler --list                    list the solved problems

            examples:
              dotnet run --project src/ProjectEulerSolver.Cli -- 42
              dotnet run --project src/ProjectEulerSolver.Cli -c Release -- --all
            """);
        return 0;
    }

    private static int List()
    {
        foreach (var problem in ProblemCatalog.All)
        {
            Console.WriteLine($"{problem.Number,3}  {problem.Title}");
        }

        return 0;
    }

    private static int RunSelected(string[] arguments)
    {
        var selected = new List<Problem>();
        foreach (var argument in arguments)
        {
            if (!int.TryParse(argument, out var number))
            {
                Console.Error.WriteLine($"'{argument}' is not a problem number.");
                return 2;
            }

            var problem = ProblemCatalog.Find(number);
            if (problem is null)
            {
                Console.Error.WriteLine($"Problem {number} has not been solved yet. Use --list to see what is available.");
                return 2;
            }

            selected.Add(problem);
        }

        return Run(selected);
    }

    private static int Run(IReadOnlyList<Problem> problems)
    {
        Console.WriteLine($"{"#",3}  {"Title",-42} {"Answer",-20} {"Time",9}");
        var failures = 0;
        var total = Stopwatch.StartNew();
        foreach (var problem in problems)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var answer = problem.Solve();
                Console.WriteLine($"{problem.Number,3}  {problem.Title,-42} {answer,-20} {stopwatch.Elapsed.TotalMilliseconds,7:N0} ms");
            }
            catch (Exception exception) when (exception is InvalidOperationException or FileNotFoundException)
            {
                failures++;
                Console.WriteLine($"{problem.Number,3}  {problem.Title,-42} FAILED: {exception.Message}");
            }
        }

        if (problems.Count > 1)
        {
            Console.WriteLine();
            Console.WriteLine($"{problems.Count - failures}/{problems.Count} solved in {total.Elapsed.TotalSeconds:N1} s");
        }

        return failures == 0 ? 0 : 1;
    }
}
