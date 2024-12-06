namespace ImageCompareCLI.utils;

public class ArgParser
{
    public static readonly Dictionary<string,string> DefaultsArgs =  new Dictionary<string, string>
    {
        {"--file1","1.jpg"},
        {"--file2","2.jpg"},
        {"--threadshold","0"},
        {"--diffcount","-1"} // max
    };
    
    public static Dictionary<string,string> Parse(string[] args)
    {
        var result = new Dictionary<string, string>();

        foreach (var arg in DefaultsArgs)
        {
            var value = args.Where(k => k.StartsWith(arg.Key))
                .FirstOrDefault() ?? $"{arg.Key}={arg.Value}"; 

            var splitValue = value.Split(new[] { '=' }, 2);

            result[arg.Key] = splitValue.Length > 1 && splitValue[0] == arg.Key
                ? splitValue[1]
                : arg.Value;
        }
        return result;
    }

    public static void ShowUsage()
    {
        Console.Write($"Usage: dotnet run ");
        foreach (var k in DefaultsArgs.Keys)
        {
            Console.Write($" {k}=<value>");
        }
        Console.WriteLine();
    }
}