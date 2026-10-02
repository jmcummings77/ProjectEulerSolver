using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver;

/// <summary>
/// Finds a problem's general solver (its public static <c>Solve(...)</c> overload) and calls it with
/// arguments supplied as text, so a problem can be run with values other than the ones in its statement.
/// </summary>
/// <remarks>
/// Supported parameter types are <see cref="int"/>, <see cref="long"/>, <see cref="BigInteger"/>,
/// <see cref="string"/>, <c>IReadOnlyList&lt;int&gt;</c> and <c>IReadOnlyList&lt;string&gt;</c>.
/// Numbers may use <c>_</c> as a digit separator. Lists are comma-separated, or <c>@path</c> to read
/// them from a file: one item per line, or Project Euler's <c>"A","B"</c> / <c>1,2,3</c> single-line formats.
/// </remarks>
public static class ParameterizedSolver
{
    /// <summary>The problem's general solver, or <c>null</c> when it has none.</summary>
    public static MethodInfo? Find(Problem problem) =>
        problem.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .SingleOrDefault(method => method.Name == nameof(Problem.Solve) && method.GetParameters().Length > 0);

    /// <summary>The general solver's parameters as text, for example <c>"limit, base"</c>; empty when there is none.</summary>
    public static string Signature(Problem problem) =>
        Find(problem) is { } method ? string.Join(", ", method.GetParameters().Select(p => p.Name)) : string.Empty;

    /// <summary>
    /// Calls the problem's general solver. Every parameter must be supplied by name, and no other names are allowed.
    /// </summary>
    /// <exception cref="ArgumentException">The problem has no general solver, or the arguments do not match its parameters.</exception>
    /// <exception cref="FormatException">An argument could not be converted to its parameter's type.</exception>
    public static object Solve(Problem problem, IReadOnlyDictionary<string, string> arguments)
    {
        var method = Find(problem)
            ?? throw new ArgumentException($"Problem {problem.Number} does not take parameters.");
        var parameters = method.GetParameters();

        var unknown = arguments.Keys.Where(name => parameters.All(p => p.Name != name)).ToArray();
        var missing = parameters.Where(p => !arguments.ContainsKey(p.Name!)).Select(p => p.Name).ToArray();
        if (unknown.Length > 0 || missing.Length > 0)
        {
            var details = new List<string>();
            if (unknown.Length > 0)
            {
                details.Add($"unknown: {string.Join(", ", unknown)}");
            }

            if (missing.Length > 0)
            {
                details.Add($"missing: {string.Join(", ", missing)}");
            }

            throw new ArgumentException(
                $"Problem {problem.Number} takes ({Signature(problem)}); {string.Join("; ", details)}.");
        }

        var values = parameters.Select(p => Convert(arguments[p.Name!], p)).ToArray();
        try
        {
            return method.Invoke(null, values)!;
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            // Surface the solver's own exception (for example an out-of-range argument), not the reflection wrapper.
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

    private static object Convert(string text, ParameterInfo parameter)
    {
        var type = parameter.ParameterType;
        try
        {
            if (type == typeof(string))
            {
                return text;
            }

            if (type == typeof(int))
            {
                return int.Parse(Plain(text), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }

            if (type == typeof(long))
            {
                return long.Parse(Plain(text), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }

            if (type == typeof(BigInteger))
            {
                return BigInteger.Parse(Plain(text), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }

            if (type == typeof(IReadOnlyList<string>))
            {
                return Items(text);
            }

            if (type == typeof(IReadOnlyList<int>))
            {
                return Items(text)
                    .Select(item => int.Parse(Plain(item), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture))
                    .ToArray();
            }
        }
        catch (Exception exception) when (exception is FormatException or OverflowException)
        {
            throw new FormatException($"'{text}' is not a valid value for {parameter.Name} ({Describe(type)}).", exception);
        }

        throw new NotSupportedException($"Parameter {parameter.Name} has unsupported type {type}.");
    }

    private static string Plain(string number) => number.Trim().Replace("_", string.Empty);

    private static string[] Items(string text)
    {
        if (!text.StartsWith('@'))
        {
            return Resources.ParseList(text);
        }

        var content = File.ReadAllText(text[1..]);
        var lines = Resources.ParseLines(content);

        // A single line holding several comma-separated items is Project Euler's list format; otherwise one item per line.
        return lines.Length == 1 && lines[0].Contains(',') ? Resources.ParseList(lines[0]) : lines;
    }

    private static string Describe(Type type) =>
        type == typeof(IReadOnlyList<int>) ? "a list of whole numbers"
        : type == typeof(IReadOnlyList<string>) ? "a list"
        : type == typeof(string) ? "text"
        : "a whole number";
}
