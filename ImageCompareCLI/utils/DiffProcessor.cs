using System.Drawing;
using ImageComparator;
// using System.Threading;

namespace ImageCompareCLI.Utils
{
    /// <summary>
    /// Processes the image comparison and manages progress display in the console.
    /// </summary>
    public class DiffProcessor
    {
        /// <summary>
        /// Interval in milliseconds for updating progress on the console.
        /// </summary>
        private const int ProgressUpdateIntervalMs = 10; // Update progress every ms

        /// <summary>
        /// Executes the image comparison process.
        /// </summary>
        /// <remarks>
        /// This method loads the input images, starts a progress display thread, 
        /// performs the image comparison using the <see cref="Comparator.ImageCompare"/> method,
        /// and saves the resulting output file to the specified location.
        /// </remarks>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Thrown if the specified input files do not exist.
        /// </exception>
        /// <example>
        /// <code>
        /// DiffProcessor.Run();
        /// </code>
        /// Output:
        /// ✔ Output file saved to output.jpg
        /// </example>
        public static void Run()
        {
            using(Bitmap bitmap1 = new Bitmap(ArgParser.Args["--file1"]))
            using(Bitmap bitmap2 = new Bitmap(ArgParser.Args["--file2"]))
            using(Bitmap bitmap3 = new Bitmap(ArgParser.Args["--file2"]))
            {
                var progressThread = ConsoleProgress();
                progressThread.Start();
                Comparator.ImageCompare(bitmap1, bitmap2, bitmap3);
                progressThread.Join();
                ConsolePrint.PrintSuccess($"Output file saved to {ArgParser.OutputFilename}");    
                bitmap3.Save(ArgParser.OutputFilename);
            }
        }

        /// <summary>
        /// Creates and starts a background thread to display the progress of the image comparison.
        /// </summary>
        /// <returns>
        /// A <see cref="Thread"/> object responsible for updating the progress in the console.
        /// </returns>
        private static Thread ConsoleProgress()
        {
            return new Thread(() =>
            {
                while (Comparator.Progress < 100)
                {
                    UpdateConsoleProgress(Comparator.Progress);
                    Thread.Sleep(ProgressUpdateIntervalMs);
                }
                UpdateConsoleProgress(100);
                Console.WriteLine("\n");
            });
        }

        /// <summary>
        /// Updates the console with the current progress of the image comparison.
        /// </summary>
        /// <param name="progress">The current progress percentage (0-100).</param>
        private static void UpdateConsoleProgress(int progress)
        {
            lock (Console.Out)
            {
                Console.CursorLeft = 0;
                Console.Write($"Progress: {progress}%");
            }
        }
    }
}
