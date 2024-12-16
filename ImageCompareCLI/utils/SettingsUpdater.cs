using ImageComparator;

namespace ImageCompareCLI.Utils
{
    public class SettingsUpdater
    {
        public static void Update()
        {
            int threadshold = int.Parse(ArgParser.Args["--threshold"]);
            int diffcount = int.Parse(ArgParser.Args["--diffcount"]);
                
            Settings.PixelBrightPercentageThreshold = threadshold;
            Settings.DiffCount = diffcount;
        }
    }    
}
