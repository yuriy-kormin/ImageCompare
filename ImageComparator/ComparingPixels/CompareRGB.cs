using System.Drawing.Imaging;

namespace ImageComparator.ComparingPixels
{
    /// <summary>
    /// A class that provides methods for comparing the RGB values of pixels between two Bitmap images.
    /// </summary>
    public class CompareRGB
    {
        /// <summary>
        /// Compares the pixel values of two images at the specified (x, y) coordinates.
        /// The comparison is based on the perceived brightness of the pixels.
        /// </summary>
        /// 
        /// <param name="x">The x-coordinate of the pixel to compare.</param>
        /// <param name="y">The y-coordinate of the pixel to compare.</param>
        /// <param name="bitmapData1">The <see cref="BitmapData"/> object of the first image.</param>
        /// <param name="bitmapData2">The <see cref="BitmapData"/> object of the second image.</param>
        /// 
        /// <returns>
        /// Returns a boolean indicating whether the brightness difference between the two pixels is within the threshold defined in <see cref="Settings.PixelBrightPercentageThreshold"/>.
        /// </returns>
        public static unsafe bool IsPixelMatch(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2)
        {
            // Assume 24-bit RGB format, hence 3 bytes per pixel.
            int bytesPerPixel = 3;
            int stride1 = bitmapData1.Stride;
            int stride2 = bitmapData2.Stride;

            byte* ptr1 = (byte*)bitmapData1.Scan0 + y * stride1 + x * bytesPerPixel;
            byte* ptr2 = (byte*)bitmapData2.Scan0 + y * stride2 + x * bytesPerPixel;

            double brightness1 = 0.299 * ptr1[2] + 0.587 * ptr1[1] + 0.114 * ptr1[0];
            double brightness2 = 0.299 * ptr2[2] + 0.587 * ptr2[1] + 0.114 * ptr2[0];

            var diff = ((double)Math.Abs(brightness1 - brightness2) / 255) * 100;
            
            return diff <= (double)Settings.PixelBrightPercentageThreshold;
        }
    }
}
