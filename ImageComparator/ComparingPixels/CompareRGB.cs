using System.Drawing.Imaging;

namespace ImageComparator.ComparingPixels
{
    public class CompareRGB
    {
        public static unsafe bool IsPixelMatch(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2)
        {
            int bytesPerPixel = 3; // Assuming 24bppRgb format
            int stride1 = bitmapData1.Stride;
            int stride2 = bitmapData2.Stride;

            byte* ptr1 = (byte*)bitmapData1.Scan0 + y * stride1 + x * bytesPerPixel;
            byte* ptr2 = (byte*)bitmapData2.Scan0 + y * stride2 + x * bytesPerPixel;

            // Get RGB values and calculate the difference
            double brightness1 = 0.299 * ptr1[2] + 0.587 * ptr1[1] + 0.114 * ptr1[0];
            double brightness2 = 0.299 * ptr2[2] + 0.587 * ptr2[1] + 0.114 * ptr2[0];


            var diff = ((double)Math.Abs(brightness1 - brightness2)/255)*100;
            return diff < (double)Settings.PixelBrightPercentageThreshold;
        }
    }    
}

