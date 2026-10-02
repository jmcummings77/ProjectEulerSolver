namespace ProjectEulerSolver;

/// <summary>
/// Base class for a single Project Euler problem. Each concrete problem is a
/// parameterless class that knows its number and title and can compute its answer.
/// </summary>
public abstract class Problem
{
    /// <summary>The problem number on projecteuler.net.</summary>
    public abstract int Number { get; }

    /// <summary>The problem's title as shown on projecteuler.net.</summary>
    public abstract string Title { get; }

    /// <summary>Link to the problem statement.</summary>
    public string Url => $"https://projecteuler.net/problem={Number}";

    /// <summary>Computes the answer. The runner and tests compare <c>ToString()</c> of the result.</summary>
    public abstract object Solve();
}
