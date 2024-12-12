namespace ImageCompareCLI.Utils
{
    public class ArgValidator
    {
        public static bool IsArgValid()
        {
        
            foreach (var filename in new[] { "--file1", "--file2" })
            {
                if ( !File.Exists(ArgParser.Args[filename]) )
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
                if (diffcount < -1 || threshold > 100)
                {
                    ConsolePrint.PrintError($"Warning: Diffcount '{diffcount}' is out of range.[-1 - 100] (-1 mean 'without a threshold')");
                    return false;
                }
            }
            return true;
        }
    }    
}

