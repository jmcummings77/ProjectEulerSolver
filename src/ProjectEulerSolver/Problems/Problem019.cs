namespace ProjectEulerSolver.Problems;

/// <summary>How many Sundays fell on the first of the month during the twentieth century.</summary>
public sealed class Problem019 : Problem
{
    public override int Number => 19;

    public override string Title => "Counting Sundays";

    public override object Solve()
    {
        var count = 0;
        for (var year = 1901; year <= 2000; year++)
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
