using System.Drawing;
using System.Drawing.Imaging;

namespace ImageComparator.Utils
{
    /// <summary>
    /// A utility class for locking bits of a Bitmap image to allow direct manipulation of pixel data.
    /// </summary>
    public class BitmapLockBits
    {
        /// <summary>
        /// Locks the bits of a Bitmap image, providing access to the pixel data in a specified lock mode.
        /// This method is used to lock the Bitmap for reading or writing pixel data directly.
        /// </summary>
        /// 
        /// <param name="bitmap">The <see cref="Bitmap"/> object whose bits are to be locked.</param>
        /// <param name="mode">The lock mode specifying whether the bits will be read or written.</param>
        ///
        /// <returns>
        /// Returns a <see cref="BitmapData"/> object that contains information about the locked bits,
        /// allowing direct manipulation of pixel data.
        /// </returns>
        public static BitmapData Lock(Bitmap bitmap, ImageLockMode mode)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            return bitmap.LockBits(rect, mode, bitmap.PixelFormat);
        }
    }
}