using System.Drawing;
using ImageCompareCLI.Utils;
using ImageComparator;

namespace ImageCompareCLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
        
            ArgParser.Parse(args);
        
            if (!ArgValidator.IsArgValid())
            {
                ConsolePrint.PrintError("\t Invalid arguments.Break execution....");
                ConsolePrint.ShowUsage();
                return;
            }
        
            using(Bitmap bitmap1 = new Bitmap(ArgParser.Args["--file1"]))
            using(Bitmap bitmap2 = new Bitmap(ArgParser.Args["--file2"]))
            {
                int threadshold = int.Parse(ArgParser.Args["--threshold"]);
                int diffcount = int.Parse(ArgParser.Args["--diffcount"]);
            
                var bitmapResult = Comparator.ImageCompare( bitmap1, bitmap2, threadshold, diffcount );
                bitmapResult.Save(ArgParser.OutputFilename);
                bitmapResult.Dispose();
                ConsolePrint.PrintSuccess($"Output file saved to {ArgParser.OutputFilename}");
            }
        
        }
    }    
}
