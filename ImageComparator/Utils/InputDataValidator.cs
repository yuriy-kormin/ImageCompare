using System.Drawing;

namespace ImageComparator.Utils
{
    public class InputDataValidator
    {
        public static void CheckSameDimensions(Bitmap bitmap1, Bitmap bitmap2)
        {
            if (bitmap1.Width != bitmap2.Width || bitmap1.Height != bitmap2.Height)
                throw new Exception("Bitmaps are different.");
        }
    }    
}

