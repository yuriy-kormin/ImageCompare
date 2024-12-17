using ImageComparator;

namespace ImageCompareCLI.Utils
{
    /// <summary>
    /// Updates the settings for the image comparison process 
    /// based on the provided command-line arguments.
    /// </summary>
    public class SettingsUpdater
    {
        /// <summary>
        /// Updates the <see cref="Settings"/> configuration using the values 
        /// parsed from the command-line arguments.
        /// </summary>
        /// <remarks>
        /// The method retrieves the values for the threshold and diff count 
        /// arguments using <see cref="ArgParser.Args"/> and assigns them 
        /// to the corresponding properties in the <see cref="Settings"/> class.
        /// </remarks>
        /// <exception cref="System.FormatException">
        /// Thrown if the argument values cannot be parsed as integers.
        /// </exception>
        /// <example>
        /// <code>
        /// SettingsUpdater.Update();
        /// Console.WriteLine(Settings.PixelBrightPercentageThreshold);
        /// Console.WriteLine(Settings.DiffCount);
        /// </code>
        /// </example>
        public static void Update()
        {
            int threadshold = int.Parse(ArgParser.Args["--threshold"]);
            int diffcount = int.Parse(ArgParser.Args["--diffcount"]);
                
            Settings.PixelBrightPercentageThreshold = threadshold;
            Settings.DiffCount = diffcount;
        }
    }    
}