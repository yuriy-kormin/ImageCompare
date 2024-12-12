using System.Drawing;

namespace ImageCompareCLI.Utils;


public class File2Bitmap
{
    public static IEnumerable<Bitmap> Open(string filePath)
    {
        using (var bitmap = new Bitmap(filePath))
        {
            yield return bitmap;
        }
        
    }

    public static void bitmapSave(Bitmap bitmap, string filePath)
    {
        try
        {
            bitmap.Save(filePath);
        }
        catch (Exception e)
        {
            ConsolePrint.PrintError(e.Message);
        }
    }
}