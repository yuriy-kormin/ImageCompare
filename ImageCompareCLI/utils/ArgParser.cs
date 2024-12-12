using System.Drawing;

namespace ImageCompareCLI.Utils
{
    public static class ArgParser
    {
        public static string OutputFilename { get; }= "output.jpg";
    
        // Dictionary to store default argument values
        public static Dictionary<string, string> Args = new()
        {
            { "--file1", "1.jpg" },
            { "--file2", "2.jpg" },
            { "--threshold", "0" }, // [0-100] percent
            { "--diffcount", "-1" } // Max [-1-100]
        };

        public static void Parse(string[] args)
        {
            foreach (var arg in args)
            {
                var split = arg.Split('=', 2);
                if (split.Length == 2 && Args.ContainsKey(split[0]))
                {
                    Args[split[0]] = split[1];
                }
                else
                {
                    ConsolePrint.PrintError($"Warning: Ignored invalid argument '{arg}'.");
                }
            }
        }

    
    }    
}
