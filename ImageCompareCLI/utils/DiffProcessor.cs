using System.Drawing;
using ImageComparator;
// using System.Threading;


namespace ImageCompareCLI.Utils
{
    

    public class DiffProcessor
    {
        private const int ProgressUpdateIntervalMs = 10; // Update progress every  ms
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

