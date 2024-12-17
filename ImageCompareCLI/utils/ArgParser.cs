namespace ImageCompareCLI.Utils
{
    /// <summary>
    /// Provides functionality to parse and validate command-line arguments 
    /// for the Image Compare CLI utility.
    /// </summary>
    public static class ArgParser
    {
        /// <summary>
        /// Gets the default name for the output image file.
        /// </summary>
        /// <value>The default filename is "output.jpg".</value>
        public static string OutputFilename { get; } = "output.jpg";

        /// <summary>
        /// A dictionary containing default argument values and their corresponding keys.
        /// </summary>
        /// <remarks>
        /// The dictionary stores the following default arguments:
        /// <list type="bullet">
        /// <item>
        /// <description><c>--file1</c>: First image file (default is "1.jpg").</description>
        /// </item>
        /// <item>
        /// <description><c>--file2</c>: Second image file (default is "2.jpg").</description>
        /// </item>
        /// <item>
        /// <description><c>--threshold</c>: Threshold percentage for comparison (default is "10").</description>
        /// </item>
        /// <item>
        /// <description><c>--diffcount</c>: Maximum difference count (-1 for unlimited, default is "-1").</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static Dictionary<string, string> Args = new()
        {
            { "--file1", "1.jpg" },
            { "--file2", "2.jpg" },
            { "--threshold", "10" }, // [0-100] percent
            { "--diffcount", "-1" } // Max [-1-100]
        };

        /// <summary>
        /// Parses the command-line arguments and updates the <see cref="Args"/> dictionary 
        /// with provided values.
        /// </summary>
        /// <param name="args">An array of command-line arguments in the format "--key=value".</param>
        /// <exception cref="ArgumentException">
        /// Thrown when invalid arguments are detected by the <see cref="ArgValidator.IsArgValid"/> method.
        /// </exception>
        /// <remarks>
        /// If an argument is invalid (e.g., incorrect format or unknown key), it will be ignored, 
        /// and a warning will be printed using <see cref="ConsolePrint.PrintError"/>.
        /// </remarks>
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

            if (!ArgValidator.IsArgValid())
            {
                throw new ArgumentException("\t Invalid arguments. Break execution....");
            }
        }
    }
}
