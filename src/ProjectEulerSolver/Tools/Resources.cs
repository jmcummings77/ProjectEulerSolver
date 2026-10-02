namespace ProjectEulerSolver.Tools;

/// <summary>Reads the puzzle data files embedded in this assembly (see the Resources folder).</summary>
public static class Resources
{
    /// <summary>Returns the full text of an embedded resource.</summary>
    public static string ReadText(string name)
    {
        using var stream = typeof(Resources).Assembly.GetManifestResourceStream(name)
            ?? throw new FileNotFoundException($"Embedded resource '{name}' was not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>Returns the non-empty lines of an embedded resource.</summary>
    public static string[] ReadLines(string name) =>
        ReadText(name)
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.Length > 0)
            .ToArray();

    /// <summary>Parses a resource in Project Euler's <c>"A","B","C"</c> list format.</summary>
    public static string[] ReadQuotedList(string name) =>
        ReadText(name)
            .Split(',')
            .Select(item => item.Trim().Trim('"'))
            .Where(item => item.Length > 0)
            .ToArray();
}
