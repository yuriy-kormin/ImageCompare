namespace ImageCompareCLI.Utils
{
    /// <summary>
    /// Provides validation logic for the command-line arguments 
    /// used in the Image Compare CLI utility.
    /// </summary>
    public class ArgValidator
    {
        /// <summary>
        /// Validates the values of the arguments provided in the <see cref="ArgParser.Args"/> dictionary.
        /// </summary>
        /// <returns>
        /// <c>true</c> if all arguments are valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The validation checks include:
        /// <list type="bullet">
        /// <item>
        /// <description>Ensuring the files specified by <c>--file1</c> and <c>--file2</c> exist on the file system.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Validating that the <c>--threshold</c> value is between 0 and 100 (inclusive).</description>
        /// </item>
        /// <item>
        /// <description>Validating that the <c>--diffcount</c> value is between -1 and 100 (inclusive), 
        /// where -1 means no threshold is applied.</description>
        /// </item>
        /// </list>
        /// If a validation check fails, an error message is printed using <see cref="ConsolePrint.PrintError"/>.
        /// </remarks>
        /// <example>
        /// Example of a valid argument set:
        /// <code>
        /// --file1=example1.jpg --file2=example2.jpg --threshold=20 --diffcount=5
        /// </code>
        /// </example>
        public static bool IsArgValid()
        {
            foreach (var filename in new[] { "--file1", "--file2" })
            {
                if (!File.Exists(ArgParser.Args[filename]))
                {
                    ConsolePrint.PrintError($"Warning: File '{ArgParser.Args[filename]}' does not exist.");
                    return false;
                }
            }

            if (int.TryParse(ArgParser.Args["--threshold"], out int threshold))
            {
                if (threshold < 0 || threshold > 100)
                {
                    ConsolePrint.PrintError($"Warning: Threshold '{threshold}' is out of range.[0-100]");
                    return false;
                }
            }

            if (int.TryParse(ArgParser.Args["--diffcount"], out int diffcount))
            {
                if (diffcount < -1 || diffcount > 100)
                {
                    ConsolePrint.PrintError($"Warning: Diffcount '{diffcount}' is out of range.[-1 - 100]" +
                                            $" (-1 means 'without a threshold')");
                    return false;
                }
            }
            return true;
        }
    }
}
