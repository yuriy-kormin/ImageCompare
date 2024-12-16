using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.ComparingPixels;

namespace ImageComparator.Utils
{
    public static class ComparingMethods
    {
        private static Func<int, int, BitmapData, BitmapData, bool> IsPixelMatch { get; set; } =
            CompareRGB.IsPixelMatch;

        public static unsafe bool IsSquareMatch(BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
        {
            int differenceCounter = 0;
            for (int j = 0; j < squareRect.Height; j++)
            {
                for (int i = 0; i < squareRect.Width; i++)
                {
                    int pixelX = squareRect.X + i;
                    int pixelY = squareRect.Y + j;

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

