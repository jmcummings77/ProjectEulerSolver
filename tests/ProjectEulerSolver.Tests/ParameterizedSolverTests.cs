using System.Numerics;
using System.Reflection;
using ProjectEulerSolver.Problems;
using Xunit;

namespace ProjectEulerSolver.Tests;

/// <summary>The reflection-based binder that lets the runner call a problem's general solver with text arguments.</summary>
public class ParameterizedSolverTests
{
    private static Dictionary<string, string> Arguments(params (string Name, string Value)[] pairs) =>
        pairs.ToDictionary(pair => pair.Name, pair => pair.Value);

    [Fact]
    public void Signature_lists_parameter_names_in_order()
    {
        Assert.Equal("limit", ParameterizedSolver.Signature(new Problem001()));
        Assert.Equal("digits, windowLength", ParameterizedSolver.Signature(new Problem008()));
        Assert.Equal("target, coins", ParameterizedSolver.Signature(new Problem031()));
    }

    [Fact]
    public void Solve_binds_whole_numbers_with_digit_separators()
    {
        Assert.Equal(23L, ParameterizedSolver.Solve(new Problem001(), Arguments(("limit", "10"))));
        Assert.Equal(233_168L, ParameterizedSolver.Solve(new Problem001(), Arguments(("limit", "1_000"))));
    }

    [Fact]
    public void Solve_binds_text_and_comma_separated_lists()
    {
        Assert.Equal(5832L, ParameterizedSolver.Solve(new Problem008(), Arguments(("digits", "1299890"), ("windowLength", "4"))));
        Assert.Equal(new BigInteger(4), ParameterizedSolver.Solve(new Problem031(), Arguments(("target", "5"), ("coins", "1, 2, 5"))));
    }

    [Fact]
    public void Solve_reads_lists_from_files_in_either_layout()
    {
        var perLine = Path.GetTempFileName();
        var singleLine = Path.GetTempFileName();
        try
        {
            File.WriteAllText(perLine, "1\n2\n\n5\n");
            File.WriteAllText(singleLine, "\"1\",\"2\",\"5\"\n");
            Assert.Equal(new BigInteger(4), ParameterizedSolver.Solve(new Problem031(), Arguments(("target", "5"), ("coins", "@" + perLine))));
            Assert.Equal(new BigInteger(4), ParameterizedSolver.Solve(new Problem031(), Arguments(("target", "5"), ("coins", "@" + singleLine))));
        }
        finally
        {
            File.Delete(perLine);
            File.Delete(singleLine);
        }
    }

    [Fact]
    public void Solve_rejects_unknown_and_missing_names()
    {
        var unknown = Assert.Throws<ArgumentException>(() => ParameterizedSolver.Solve(new Problem001(), Arguments(("limit", "10"), ("base", "2"))));
        Assert.Contains("unknown: base", unknown.Message);

        var missing = Assert.Throws<ArgumentException>(() => ParameterizedSolver.Solve(new Problem008(), Arguments(("windowLength", "4"))));
        Assert.Contains("missing: digits", missing.Message);
    }

    [Fact]
    public void Solve_reports_values_that_do_not_fit_the_parameter()
    {
        var exception = Assert.Throws<FormatException>(() => ParameterizedSolver.Solve(new Problem001(), Arguments(("limit", "ten"))));
        Assert.Contains("limit", exception.Message);
    }

    [Fact]
    public void Solve_surfaces_the_solvers_own_argument_checks() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => ParameterizedSolver.Solve(new Problem001(), Arguments(("limit", "-1"))));

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(4, 3)]
    [InlineData(10, 23)] // The example in the problem statement.
    [InlineData(16, 60)]
    [InlineData(1_000_000_000, 233_333_333_166_666_668)]
    public void Problem001_matches_the_statement_and_brute_force(long limit, long expected)
    {
        Assert.Equal(expected, Problem001.Solve(limit));
        if (limit <= 1000)
        {
            Assert.Equal(Enumerable.Range(0, (int)limit).Where(n => n % 3 == 0 || n % 5 == 0).Sum(), Problem001.Solve(limit));
        }
    }

    [Fact]
    public void Problem008_matches_the_statement_example()
    {
        var digits = string.Concat(ProjectEulerSolver.Tools.Resources.ReadLines("0008_number.txt"));
        Assert.Equal(5832, Problem008.Solve(digits, 4)); // 9 × 9 × 8 × 9
        Assert.Equal(9, Problem008.Solve("3675356291", 1));
        Assert.Equal(0, Problem008.Solve("1010", 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem008.Solve("123", 4));
        Assert.Throws<ArgumentException>(() => Problem008.Solve("12a", 2));
    }

    /// <summary>Problems whose statement fixes nothing that could sensibly vary, so they have no general solver.</summary>
    private static readonly int[] WithoutParameters = [33, 34, 44, 46, 63];

    private static readonly Type[] AllowedParameterTypes =
    [
        typeof(int),
        typeof(long),
        typeof(BigInteger),
        typeof(string),
        typeof(IReadOnlyList<int>),
        typeof(IReadOnlyList<string>),
    ];

    [Fact]
    public void Every_problem_follows_the_general_solver_convention()
    {
        foreach (var problem in ProblemCatalog.All)
        {
            var overloads = problem.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(method => method.Name == nameof(Problem.Solve))
                .ToArray();

            if (WithoutParameters.Contains(problem.Number))
            {
                Assert.Empty(overloads);
                Assert.Null(ParameterizedSolver.Find(problem));
                continue;
            }

            var solver = Assert.Single(overloads);
            Assert.Same(solver, ParameterizedSolver.Find(problem));
            Assert.NotEqual(typeof(object), solver.ReturnType);
            Assert.NotEqual(typeof(void), solver.ReturnType);
            Assert.NotEmpty(solver.GetParameters());
            Assert.All(solver.GetParameters(), parameter =>
            {
                Assert.Contains(parameter.ParameterType, AllowedParameterTypes);
                Assert.False(parameter.HasDefaultValue, $"Problem {problem.Number}: {parameter.Name} has a default value.");
            });
        }
    }

    [Fact]
    public void Problem031_rejects_targets_outside_its_range()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem031.Solve(-1, [1, 2]));
        Assert.Throws<ArgumentOutOfRangeException>(() => Problem031.Solve(1_000_001, [1, 2]));
        Assert.Equal(new BigInteger(500_001), Problem031.Solve(1_000_000, [1, 2])); // One way for each number of 2s from 0 to 500,000.
    }

    [Fact]
    public void Problem031_counts_small_cases_by_hand()
    {
        Assert.Equal(new BigInteger(1), Problem031.Solve(0, [1, 2]));
        Assert.Equal(new BigInteger(4), Problem031.Solve(5, [1, 2, 5])); // 5; 2+2+1; 2+1+1+1; 1+1+1+1+1
        Assert.Equal(new BigInteger(0), Problem031.Solve(3, [2]));
        Assert.Throws<ArgumentException>(() => Problem031.Solve(5, [1, 1]));
    }
}
