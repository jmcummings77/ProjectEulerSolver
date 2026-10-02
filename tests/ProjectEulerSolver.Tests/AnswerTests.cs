using Xunit;

namespace ProjectEulerSolver.Tests;

/// <summary>Checks every solved problem against its accepted answer.</summary>
public class AnswerTests
{
    private static readonly Dictionary<int, string> Expected = new()
    {
        [1] = "233168",
        [2] = "4613732",
        [3] = "6857",
        [4] = "906609",
        [5] = "232792560",
        [6] = "25164150",
        [7] = "104743",
        [8] = "23514624000",
        [9] = "31875000",
        [10] = "142913828922",
        [11] = "70600674",
        [12] = "76576500",
        [13] = "5537376230",
        [14] = "837799",
        [15] = "137846528820",
        [16] = "1366",
        [17] = "21124",
        [18] = "1074",
        [19] = "171",
        [20] = "648",
        [21] = "31626",
        [22] = "871198282",
        [23] = "4179871",
        [24] = "2783915460",
        [25] = "4782",
        [26] = "983",
        [27] = "-59231",
        [28] = "669171001",
        [29] = "9183",
        [30] = "443839",
        [31] = "73682",
        [32] = "45228",
        [33] = "100",
        [34] = "40730",
        [35] = "55",
        [36] = "872187",
        [37] = "748317",
        [38] = "932718654",
        [39] = "840",
        [40] = "210",
        [41] = "7652413",
        [42] = "162",
        [43] = "16695334890",
        [44] = "5482660",
        [45] = "1533776805",
        [46] = "5777",
        [47] = "134043",
        [48] = "9110846700",
        [49] = "296962999629",
        [50] = "997651",
        [51] = "121313",
        [52] = "142857",
        [53] = "4075",
        [54] = "376",
        [55] = "249",
        [56] = "972",
        [57] = "153",
        [58] = "26241",
        [59] = "107359", // For 0059_cipher_original.txt; the site replaced the cipher in 2019 (its answer is 129448).
        [60] = "26033",
        [61] = "28684",
        [62] = "127035954683",
        [63] = "49",
        [64] = "1322",
        [65] = "272",
        [66] = "661",
        [67] = "7273",
        [68] = "6531031914842725",
        [69] = "510510",
        [70] = "8319823",
        [71] = "428570",
        [72] = "303963552391",
        [73] = "7295372",
        [74] = "402",
        [75] = "161667",
        [76] = "190569291",
        [77] = "71",
        [78] = "55374",
        [79] = "73162890",
    };

    public static TheoryData<int> ProblemNumbers => new(ProblemCatalog.All.Select(p => p.Number));

    [Theory]
    [MemberData(nameof(ProblemNumbers))]
    public void Problem_produces_the_accepted_answer(int number)
    {
        var problem = ProblemCatalog.Find(number);
        Assert.NotNull(problem);
        Assert.True(Expected.ContainsKey(number), $"No expected answer recorded for problem {number}.");

        Assert.Equal(Expected[number], problem.Solve().ToString());
    }

    [Fact]
    public void Every_expected_answer_has_a_solution()
    {
        var solved = ProblemCatalog.All.Select(p => p.Number).ToHashSet();
        Assert.Empty(Expected.Keys.Where(number => !solved.Contains(number)));
    }

    [Fact]
    public void Problem_numbers_are_unique_and_match_their_class_names()
    {
        foreach (var problem in ProblemCatalog.All)
        {
            Assert.Equal($"Problem{problem.Number:000}", problem.GetType().Name);
        }

        Assert.Equal(ProblemCatalog.All.Count, ProblemCatalog.All.Select(p => p.Number).Distinct().Count());
    }

    [Fact]
    public void Every_problem_has_a_title()
    {
        Assert.All(ProblemCatalog.All, problem => Assert.False(string.IsNullOrWhiteSpace(problem.Title)));
    }
}
