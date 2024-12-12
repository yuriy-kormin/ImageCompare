using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.ComparingPixels;

namespace ImageComparator.Utils
{
    public static class ComparingMethods
    {
        private static Func<int, int, BitmapData, BitmapData, bool> IsPixelMatch { get; set; } =
            CompareRGB.IsPixelMatch;

        // public static bool IsSquareMatchWithDrift(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
        // {
        //     // int driftRange = squareSize/2;
        //     
        //     // here is some method to set proper PixelComparator
        //     
        //     int driftRange = 1;
        //
        //     for (int dy = 0; dy <= driftRange; dy++)
        //     {
        //         for (int dx = 0; dx <= driftRange; dx++)
        //         {
        //             if (IsDriftMatch(x, dx, y, dy, bitmapData1, bitmapData2, squareRect))
        //             {
        //                 return true;
        //             }
        //         }
        //     }
        //
        //     return false;
        // }
        //
        // static bool IsDriftMatch(int x, int dx, int y, int dy, BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
        // {
        //     return IsSquareMatch(x+dx, y+dy, bitmapData1, bitmapData2, squareRect)|| 
        //            IsSquareMatch(x-dx, y-dy, bitmapData1, bitmapData2, squareRect);
        // }
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

