namespace ProjectEulerSolver.Problems;

/// <summary>How many Sundays fell on the first of the month during the twentieth century.</summary>
public sealed class Problem019 : Problem
{
    public override int Number => 19;

    public override string Title => "Counting Sundays";

    public override object Solve() => Solve(startYear: 1901, endYear: 2000);

    /// <summary>
    /// How many Sundays fall on the first of a month from 1 January <paramref name="startYear"/> to
    /// 31 December <paramref name="endYear"/>. Dates follow the Gregorian leap-year rule given in the statement
    /// throughout, including for years before that calendar was adopted.
    /// </summary>
    /// <param name="startYear">The first year counted, from 1 to 9999 (the years <see cref="DateOnly"/> covers).</param>
    /// <param name="endYear">The last year counted, from <paramref name="startYear"/> to 9999.</param>
    public static int Solve(int startYear, int endYear)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(startYear, DateOnly.MinValue.Year);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(startYear, DateOnly.MaxValue.Year);
        ArgumentOutOfRangeException.ThrowIfLessThan(endYear, startYear);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(endYear, DateOnly.MaxValue.Year);

        var count = 0;
        for (var year = startYear; year <= endYear; year++)
        {
            for (var month = 1; month <= 12; month++)
            {
                if (new DateOnly(year, month, 1).DayOfWeek == DayOfWeek.Sunday)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
