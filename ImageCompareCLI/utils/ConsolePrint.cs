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

class ConsoleUtility
{
    const char _block = '■';
    const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
    const string _twirl = "-\\|/";

    public static void WriteProgressBar(int percent, bool update = false)
    {
        if (update)
            Console.Write(_back);
        Console.Write("[");
        var p = (int)((percent / 10f) + .5f);
        for (var i = 0; i < 10; ++i)
        {
            if (i >= p)
                Console.Write(' ');
            else
                Console.Write(_block);
        }
        Console.Write("] {0,3:##0}%", percent);
    }

    public static void WriteProgress(int progress, bool update = false)
    {
        if (update)
            Console.Write("\b");
        Console.Write(_twirl[progress % _twirl.Length]);
    }
}
