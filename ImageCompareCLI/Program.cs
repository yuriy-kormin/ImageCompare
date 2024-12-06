using ImageCompareCLI.utils;

public class Program
{
    public static void Main(string[] args)
    {
        var Keys = ArgParser.Parse(args);
        foreach (var VARIABLE in Keys)
        {
            Console.WriteLine(VARIABLE);   
        }

        ArgParser.ShowUsage();
    }
}