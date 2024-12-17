namespace ImageCompareCLI.Utils
{
    /// <summary>
    /// Provides methods for printing formatted messages to the console, 
    /// including errors, successes, and usage instructions.
    /// </summary>
    public class ConsolePrint
    {
        /// <summary>
        /// Prints an error message to the console in red with a '✘' prefix.
        /// </summary>
        /// <param name="error">The error message to display.</param>
        /// <example>
        /// <code>
        /// ConsolePrint.PrintError("File not found.");
        /// </code>
        /// Output:
        /// ✘ File not found.
        /// </example>
        public static void PrintError(string error)
        {
            Console.WriteLine($"\x1b[31m✘\x1b[0m {error}");
        }

        /// <summary>
        /// Prints a success message to the console in green with a '✔' prefix.
        /// </summary>
        /// <param name="message">The success message to display.</param>
        /// <example>
        /// <code>
        /// ConsolePrint.PrintSuccess("File processed successfully.");
        /// </code>
        /// Output:
        /// ✔ File processed successfully.
        /// </example>
        public static void PrintSuccess(string message)
        {
            Console.WriteLine($"\x1b[32m\u2714\x1b[0m {message}");
        }

        /// <summary>
        /// Displays usage instructions for the command-line arguments 
        /// expected by the application.
        /// </summary>
        /// <remarks>
        /// The usage instructions include the list of required arguments 
        /// and their expected values.
        /// </remarks>
        /// <example>
        /// <code>
        /// ConsolePrint.ShowUsage();
        /// </code>
        /// Output:
        /// ! Usage: dotnet run --file1=&lt;value&gt; --file2=&lt;value&gt; --threshold=&lt;value&gt; --diffcount=&lt;value&gt;
        /// </example>
        public static void ShowUsage()
        {
            var usage = string.Join(" ", ArgParser.Args.Keys.Select(k => $"{k}=<value>"));
            Console.WriteLine($"\x1b[33m!\x1b[0m Usage: dotnet run {usage}");
        }
    }
}
