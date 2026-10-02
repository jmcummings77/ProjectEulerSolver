using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The shortest passcode consistent with fifty successful three-character login attempts.</summary>
public sealed class Problem079 : Problem
{
    public override int Number => 79;

    public override string Title => "Passcode Derivation";

    public override object Solve() => Solve(attempts: Resources.ReadLines("0079_keylog.txt"));

    /// <summary>
    /// The shortest passcode consistent with <paramref name="attempts"/>, each of which gives some of the
    /// passcode's digits in the order they appear in it, assuming that no digit occurs twice in the passcode.
    /// When several passcodes of that length fit, the one that sorts first is returned.
    /// </summary>
    /// <param name="attempts">At least one login attempt; each is a non-empty string of decimal digits, of any length.</param>
    /// <exception cref="InvalidOperationException">The attempts contradict each other, so no passcode without a repeated digit fits.</exception>
    public static string Solve(IReadOnlyList<string> attempts)
    {
        if (attempts.Count == 0 || attempts.Any(attempt => attempt.Length == 0 || attempt.Any(c => c is < '0' or > '9')))
        {
            throw new ArgumentException("Expected at least one attempt, each a non-empty string of decimal digits.", nameof(attempts));
        }

        return DerivePasscode(attempts);
    }

    /// <summary>
    /// Each attempt says "a comes before b before c". Assuming no digit repeats, the passcode needs every digit
    /// seen exactly once, so the shortest one is a topological order of that precedence graph (Kahn's algorithm,
    /// smallest digit first on ties).
    /// </summary>
    internal static string DerivePasscode(IEnumerable<string> loginAttempts)
    {
        var attempts = loginAttempts.Distinct().ToArray();
        var digits = attempts.SelectMany(a => a).Distinct().Order().ToList();
        var predecessors = digits.ToDictionary(d => d, _ => new HashSet<char>());
        foreach (var attempt in attempts)
        {
            // Neighbouring pairs are enough: the order is transitive. A digit repeated within one attempt ends
            // up before itself, which the cycle check below reports.
            for (var i = 1; i < attempt.Length; i++)
            {
                predecessors[attempt[i]].Add(attempt[i - 1]);
            }
        }

        var passcode = string.Empty;
        while (predecessors.Count > 0)
        {
            var ready = predecessors.Where(kv => kv.Value.Count == 0).Select(kv => kv.Key).ToArray();
            if (ready.Length == 0)
            {
                throw new InvalidOperationException("The login attempts imply a cyclic ordering, so no passcode is consistent with them.");
            }

            var next = ready.Min();
            passcode += next;
            predecessors.Remove(next);
            foreach (var remaining in predecessors.Values)
            {
                remaining.Remove(next);
            }
        }

        return passcode;
    }
}
