using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;
using ImageComparator.Filters;

namespace ImageComparator
{
    public class Settings
    {
        public static int squareSize { get; } = 15;
        public static int PixelCounterPercentageThreshold { get; } = 20;
        public static double GaussianSigma { get; } = 2.0f;
        public static int PixelBrightPercentageThreshold { get; set; } = 10; // overwritten by input theadshold
        public static int DiffCount { get; set; } = -1; // overwritten by input theadshold
    }

    public class Comparator
    {
        public static Bitmap ImageCompare(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmapResult)
        {
            InputDataValidator.CheckSameDimensions(bitmap1, bitmap2);
            int width = bitmap1.Width;
            int height = bitmap1.Height;

            // var bitmapResult = new Bitmap(bitmap2, bitmap2.Width,bitmap2.Height);
            // var bitmapResult = new Bitmap(bitmap2, width, height);
            
            foreach (Bitmap bm in new[] { bitmap1, bitmap2 })
            {
                GaussBlur.ApplyGaussianBlur(bm, Settings.GaussianSigma);
            }

            BitmapData? bitmapData1 = null;
            BitmapData? bitmapData2 = null;
            BitmapData? bitmapDataResult = null;
            
            
            try
            {
                
                bitmapData1 = BitmapLockBits.Lock(bitmap1, ImageLockMode.ReadOnly);
                bitmapData2 = BitmapLockBits.Lock(bitmap2, ImageLockMode.ReadOnly);
                bitmapDataResult = BitmapLockBits.Lock(bitmapResult, ImageLockMode.WriteOnly);

                unsafe
                {
                    byte* resultPtr = (byte*)bitmapDataResult.Scan0;

                    // Iterate through squares
                    for (int y = 0; y < height; y += Settings.squareSize)
                    {
                        for (int x = 0; x < width; x += Settings.squareSize)
                        {
                            int currentSquareWidth = Math.Min(Settings.squareSize, width - x);
                            int currentSquareHeight = Math.Min(Settings.squareSize, height - y);
                            Rectangle squareRect = new Rectangle(x, y, currentSquareWidth, currentSquareHeight);

                            if (!ComparingMethods.IsSquareMatch(x, y, bitmapData1, bitmapData2, squareRect))
                            {
                                // DrawRectangles.DrawBorder(resultPtr, x, y, currentSquareWidth, currentSquareHeight, bitmapDataResult.Stride, 3);
                                CombineSquares.AddOrExtend(x, y, x + currentSquareWidth, y + currentSquareHeight);
                            }
                        }
                    }

                    foreach (var square in CombineSquares.Squares)
                    {
                        DrawRectangles.DrawBorder(resultPtr, square.x1, square.y1, square.x2 - square.x1,
                            square.y2 - square.y1, bitmapDataResult.Stride, 3);
                    }
                }

            }
            finally
            {
                if (bitmapData1 != null)
                {
                    bitmap1.UnlockBits(bitmapData1);
                }

                if (bitmapData2 != null)
                {
                    bitmap2.UnlockBits(bitmapData2);
                }

                if (bitmapDataResult != null)
                {
                    bitmapResult.UnlockBits(bitmapDataResult);
                }
            }

            return bitmapResult;
        }
    }

}


