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
            using(Bitmap resultBitmap = new Bitmap(ArgParser.OutputFilename))
            {
                int threadshold = int.Parse(ArgParser.Args["--threshold"]);
                int diffcount = int.Parse(ArgParser.Args["--diffcount"]);
            
                Comparator.ImageCompare( bitmap1, bitmap2, resultBitmap, threadshold, diffcount );
            }
        
        }
    }    
}
