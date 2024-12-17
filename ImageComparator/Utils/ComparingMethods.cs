using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.ComparingPixels;

namespace ImageComparator.Utils
{
    /// <summary>
    /// A utility class that provides methods for comparing pixels in images.
    /// </summary>
    public static class ComparingMethods
    {
        /// <summary>
        /// A function to compare pixels. The default comparison method is RGB-based.
        /// </summary>
        private static Func<int, int, BitmapData, BitmapData, bool> IsPixelMatch { get; set; } =
            CompareRGB.IsPixelMatch;

        /// <summary>
        /// Compares a rectangular area (square) in two images to check if the difference in pixels is within a threshold.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the square in the first image.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the square in the first image.</param>
        /// <param name="bitmapData1">The first bitmap's data to compare.</param>
        /// <param name="bitmapData2">The second bitmap's data to compare.</param>
        /// <param name="squareRect">The rectangle defining the square area to compare.</param>
        /// <returns>Returns true if the percentage of pixel differences is within the allowed threshold; otherwise, false.</returns>
        public static unsafe bool IsSquareMatch(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
        {
            int differenceCounter = 0;
            for (int j = 0; j < squareRect.Height; j++)
            {
                for (int i = 0; i < squareRect.Width; i++)
                {
                    int pixelX = x + i;
                    int pixelY = y + j;

                    if (!IsPixelMatch(pixelX, pixelY, bitmapData1, bitmapData2))
                    {
                        differenceCounter++;
                    }
                }
            }
            var pixelCount = squareRect.Height * squareRect.Width;
            return ((double)differenceCounter / pixelCount) * 100 <= Settings.PixelCounterPercentageThreshold;
        }
    }    
}
