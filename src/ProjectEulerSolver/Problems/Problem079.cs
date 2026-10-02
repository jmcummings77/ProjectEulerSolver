using ProjectEulerSolver.Tools;

namespace ProjectEulerSolver.Problems;

/// <summary>The shortest passcode consistent with fifty successful three-character login attempts.</summary>
public sealed class Problem079 : Problem
{
    public override int Number => 79;

    public override string Title => "Passcode Derivation";

    public override object Solve() => DerivePasscode(Resources.ReadLines("0079_keylog.txt"));

    /// <summary>
    /// Each attempt says "a comes before b before c". Assuming no digit repeats, the shortest passcode is the
    /// topological order of that precedence graph (Kahn's algorithm, smallest digit first on ties).
    /// </summary>
    internal static string DerivePasscode(IEnumerable<string> loginAttempts)
    {
        var attempts = loginAttempts.Distinct().ToArray();
        var digits = attempts.SelectMany(a => a).Distinct().Order().ToList();
        var predecessors = digits.ToDictionary(d => d, _ => new HashSet<char>());
        foreach (var attempt in attempts)
        {
            predecessors[attempt[1]].Add(attempt[0]);
            predecessors[attempt[2]].Add(attempt[0]);
            predecessors[attempt[2]].Add(attempt[1]);
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
