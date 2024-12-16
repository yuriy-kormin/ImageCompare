namespace ImageCompareCLI.Utils
{
    public class ConsolePrint
    {
        public static void PrintError(string error)
        {
            Console.WriteLine($"\x1b[31m✘\x1b[0m {error}");
        }
        public static void PrintSuccess(string message)
        {
            Console.WriteLine($"\x1b[32m\u2714\x1b[0m {message}");
        }
    
        public static void ShowUsage()
        {
            var usage = string.Join(" ", ArgParser.Args.Keys.Select(k => $"{k}=<value>"));
            Console.WriteLine($"\x1b[33m!\x1b[0m Usage: dotnet run {usage}");
        }
    }    
}
