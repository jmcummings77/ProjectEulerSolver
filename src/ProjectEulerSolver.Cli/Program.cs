using System.Diagnostics;

namespace ProjectEulerSolver.Cli;

/// <summary>Command-line runner: solves one, several or all problems and reports timings.</summary>
public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length == 0 || args[0] is "--help" or "-h")
        {
            return Usage();
        }

        return args[0] switch
        {
            "--all" => Run(ProblemCatalog.All),
            "--list" => List(),
            _ => RunSelected(args),
        };
    }

    private static int Usage()
    {
        Console.WriteLine("""
            Project Euler solver

            usage:
              euler <number> [<number> ...]     solve the given problem(s)
              euler <number> <name>=<value>...  solve one problem with other values than its statement's
              euler --all                       solve every problem and report timings
              euler --list                      list the solved problems and the parameters they take

            Parameter values are whole numbers (1_000_000 is fine), text, or lists: comma-separated,
            or @path to read a list from a file.

            examples:
              dotnet run --project src/ProjectEulerSolver.Cli -- 42
              dotnet run --project src/ProjectEulerSolver.Cli -- 1 limit=1_000_000
              dotnet run --project src/ProjectEulerSolver.Cli -- 31 target=500 coins=1,2,5,10,20,50,100,200
              dotnet run --project src/ProjectEulerSolver.Cli -c Release -- --all
            """);
        return 0;
    }

    private static int List()
    {
        foreach (var problem in ProblemCatalog.All)
        {
            Console.WriteLine($"{problem.Number,3}  {problem.Title,-42} {problem.Url,-36} {ParameterizedSolver.Signature(problem)}");
        }

        return 0;
    }

    private static int RunSelected(string[] arguments)
    {
        var selected = new List<Problem>();
        var parameters = new Dictionary<string, string>();
        foreach (var argument in arguments)
        {
            // A value may be negative or a path, so only the part before '=' has to look like a name.
            var separator = argument.IndexOf('=');
            if (separator > 0 && char.IsLetter(argument[0]))
            {
                parameters[argument[..separator]] = argument[(separator + 1)..];
                continue;
            }

            if (argument.StartsWith('-'))
            {
                Console.Error.WriteLine($"Unknown option '{argument}'. Use --help to see the available options.");
                return 2;
            }

            if (!int.TryParse(argument, out var number) || number < 1)
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

        if (parameters.Count == 0)
        {
            return Run(selected);
        }

        if (selected.Count != 1)
        {
            Console.Error.WriteLine("Parameters apply to exactly one problem: give one problem number followed by name=value pairs.");
            return 2;
        }

        return RunWith(selected[0], parameters);
    }

    private static int RunWith(Problem problem, Dictionary<string, string> parameters)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var answer = ParameterizedSolver.Solve(problem, parameters);
            Console.WriteLine($"{answer}");
            Console.Error.WriteLine($"Problem {problem.Number} ({problem.Title}) solved in {stopwatch.Elapsed.TotalMilliseconds:N0} ms");
            return 0;
        }
        catch (Exception exception) when (exception is ArgumentException or FormatException or IOException or InvalidOperationException)
        {
            // Bad arguments, an unreadable @file, or arguments for which the problem has no answer.
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
    }

    private static int Run(IReadOnlyList<Problem> problems)
    {
        Console.WriteLine($"{"#",3}  {"Title",-42} {"Answer",-20} {"Time",10}");
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
            catch (Exception exception)
            {
                // A batch run should report every failure and still print the summary, so catch everything here.
                failures++;
                Console.WriteLine($"{problem.Number,3}  {problem.Title,-42} FAILED");
                Console.Error.WriteLine($"Problem {problem.Number} failed: {exception.GetType().Name}: {exception.Message}");
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
