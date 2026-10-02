using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The millionth lexicographic permutation of the digits 0 through 9.</summary>
public sealed class Problem024 : Problem
{
    public override int Number => 24;

    public override string Title => "Lexicographic Permutations";

    public override object Solve()
    {
        // Factorial number system: the k-th permutation (0-based) picks digit index k / (n-1)!,
        // then recurses on the remainder with the remaining digits.
        var remaining = Enumerable.Range(0, 10).ToList();
        var index = 999_999L;
        var result = new char[10];
        for (var position = 0; position < result.Length; position++)
        {
            var blockSize = (long)NumberTheory.Factorial(remaining.Count - 1);
            var pick = (int)(index / blockSize);
            index %= blockSize;
            result[position] = (char)('0' + remaining[pick]);
            remaining.RemoveAt(pick);
        }

        return new string(result);
    }
}
