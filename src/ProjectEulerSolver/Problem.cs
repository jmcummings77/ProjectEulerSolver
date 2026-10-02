namespace ProjectEulerSolver;

/// <summary>
/// Base class for a single Project Euler problem. Each concrete problem is a
/// parameterless class that knows its number and title and can compute its answer.
/// </summary>
/// <remarks>
/// Most problems also expose a general solver as a public static <c>Solve(...)</c> overload whose
/// parameters are the quantities the problem statement fixes (a limit, a digit count, the input data).
/// The parameterless <see cref="Solve"/> calls it with the statement's values;
/// <see cref="ParameterizedSolver"/> finds it by reflection so the runner can accept other values.
/// </remarks>
public abstract class Problem
{
    /// <summary>The problem number on projecteuler.net.</summary>
    public abstract int Number { get; }

    /// <summary>The problem's title as shown on projecteuler.net.</summary>
    public abstract string Title { get; }

    /// <summary>Link to the problem statement.</summary>
    public string Url => $"https://projecteuler.net/problem={Number}";

    /// <summary>
    /// Computes the accepted answer, using the values given in the problem statement.
    /// The runner and tests compare <c>ToString()</c> of the result.
    /// </summary>
    public abstract object Solve();
}
