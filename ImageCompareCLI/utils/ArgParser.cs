namespace ImageCompareCLI.Utils;

public static class ArgParser
{
    // Dictionary to store default argument values
    public static readonly Dictionary<string, string> DefaultArgs = new()
    {
        { "--file1", "1.jpg" },
        { "--file2", "2.jpg" },
        { "--threshold", "0" },
        { "--diffcount", "-1" } // Max
    };

    public static Dictionary<string, string> Parse(string[] args)
    {
        var result = new Dictionary<string, string>();

        foreach (var (key, defaultValue) in DefaultArgs)
        {
            // Find the argument in the input, or use the default
            var inputArg = args.FirstOrDefault(arg => arg.StartsWith($"{key}="));
            var value = inputArg?.Split('=', 2)[1] ?? defaultValue;
            result[key] = value;
        }

        return result;
    }

    public static void ShowUsage()
    {
        var usage = string.Join(" ", DefaultArgs.Keys.Select(k => $"{k}=<value>"));
        Console.WriteLine($"Usage: dotnet run {usage}");
    }
}