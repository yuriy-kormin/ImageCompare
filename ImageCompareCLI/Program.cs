using ImageCompareCLI.Utils;


namespace ImageCompareCLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ArgParser.Parse(args);
                SettingsUpdater.Update();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                ConsolePrint.ShowUsage();
                Environment.Exit(1);
            }
            
            DiffProcessor.Run();
        }
    }    
}
