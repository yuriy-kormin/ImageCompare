using System.Drawing;
using System.Drawing.Imaging;

namespace ImageComparator.Utils
{
    public class BitmapLockBits
    {
        public static BitmapData Lock(Bitmap bitmap,ImageLockMode mode,Rectangle rect = default)
        {
            if (rect == default)
            {
                rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            }
            return bitmap.LockBits(rect, mode, bitmap.PixelFormat);
        }

    }    
}

