using System.Diagnostics;

namespace ProjectEulerSolver.Tools;

/// <summary>Spells out integers in British English ("one hundred and fifteen").</summary>
public static class NumberWords
{
    private static readonly string[] Units =
    [
        "zero",
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
        "ten",
        "eleven",
        "twelve",
        "thirteen",
        "fourteen",
        "fifteen",
        "sixteen",
        "seventeen",
        "eighteen",
        "nineteen",
    ];

    private static readonly string[] Tens =
    [
        "",
        "",
        "twenty",
        "thirty",
        "forty",
        "fifty",
        "sixty",
        "seventy",
        "eighty",
        "ninety",
    ];

    private static readonly (int Value, string Name)[] Scales =
    [
        (1_000_000_000, "billion"),
        (1_000_000, "million"),
        (1_000, "thousand"),
        (100, "hundred"),
    ];

    /// <summary>Spells out <paramref name="n"/>, which must be non-negative.</summary>
    public static string ToWords(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n));
        }

        if (n < 20)
        {
            return Units[n];
        }

        if (n < 100)
        {
            return n % 10 == 0 ? Tens[n / 10] : $"{Tens[n / 10]}-{Units[n % 10]}";
        }

        foreach (var (value, name) in Scales)
        {
            if (n < value)
            {
                continue;
            }

            var leading = $"{ToWords(n / value)} {name}";
            var remainder = n % value;
            if (remainder == 0)
            {
                return leading;
            }

            var joiner = remainder < 100 ? " and " : " ";
            return leading + joiner + ToWords(remainder);
        }

        throw new UnreachableException();
    }
}
