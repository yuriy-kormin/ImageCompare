using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;
using ImageComparator.Filters;

namespace ImageComparator
{
    public class Comparator
    {
        private static int squareSize { get; } = 15;
        private static int PixelCounterPercentageThreshold { get; } = 20;
        private static double GaussianSigma { get; } = 2.0f;
        private static int PixelBrightPercentageThreshold { get; set; } = 10; // overwritten by input theadshold

        public static Bitmap ImageCompare(Bitmap bitmap1, Bitmap bitmap2, int threshold,int diffcount)
        {
            PixelBrightPercentageThreshold = threshold;
            var bitmapResult = new Bitmap(bitmap2);
            
            foreach (Bitmap bm in new[] { bitmap1, bitmap2 })
            {
                GaussBlur.ApplyGaussianBlur(bm, GaussianSigma);    
            }
            bitmap1.Save("some_fn.jpg");
            
            Rectangle fullRect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
            
            BitmapData bitmapData1 = null;
            BitmapData bitmapData2 = null;
            BitmapData bitmapDataResult = null;
            
            try
            {
                int width = bitmap1.Width;
                int height = bitmap1.Height;

                bitmapData1 = BitmapLockBits.Lock(bitmap1, ImageLockMode.ReadOnly);
                bitmapData2 = BitmapLockBits.Lock(bitmap2, ImageLockMode.ReadOnly);
                bitmapDataResult = BitmapLockBits.Lock(bitmapResult, ImageLockMode.WriteOnly);
                    
                unsafe
                {
                    byte* resultPtr = (byte*)bitmapDataResult.Scan0;

                    // Iterate through squares
                    for (int y = 0; y < height; y += squareSize)
                    {
                        for (int x = 0; x < width; x += squareSize)
                        {
                            int currentSquareWidth = Math.Min(squareSize, width - x);
                            int currentSquareHeight = Math.Min(squareSize, height - y);
                            Rectangle squareRect = new Rectangle(x, y, currentSquareWidth, currentSquareHeight);

                            if (!IsSquareMatchWithDrift(x, y, bitmapData1, bitmapData2, squareRect))
                            {
                                DrawRectangles.DrawBorder(resultPtr, x, y, currentSquareWidth, currentSquareHeight, bitmapDataResult.Stride, 3);
                                CombineSquares.AddOrExtend(x, y, x+currentSquareWidth, y+currentSquareHeight);
                            }
                        }
                    }
                    
                    foreach (var square in CombineSquares.Squares)
                    {
                        DrawRectangles.DrawBorder(resultPtr, square.x1, square.y1, square.x2 - square.x1, square.y2-square.y1, bitmapDataResult.Stride, 3, new List<int>(){255,0,0});    
                    }
                }
                
            }
            finally
            {
                if (bitmapData1 !=null){bitmap1.UnlockBits(bitmapData1);}
                if (bitmapData2 !=null){bitmap2.UnlockBits(bitmapData2);}
                if (bitmapDataResult !=null){bitmapResult.UnlockBits(bitmapDataResult);}
            }

            return bitmapResult;
        }

        

        // Check if a single pixel has a significant difference
        static unsafe bool IsPixelMatch(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2)
        {
            int bytesPerPixel = 3; // Assuming 24bppRgb format
            int stride1 = bitmapData1.Stride;
            int stride2 = bitmapData2.Stride;

            byte* ptr1 = (byte*)bitmapData1.Scan0 + y * stride1 + x * bytesPerPixel;
            byte* ptr2 = (byte*)bitmapData2.Scan0 + y * stride2 + x * bytesPerPixel;

            // Get RGB values and calculate the difference
            double brightness1 = 0.299 * ptr1[2] + 0.587 * ptr1[1] + 0.114 * ptr1[0];
            double brightness2 = 0.299 * ptr2[2] + 0.587 * ptr2[1] + 0.114 * ptr2[0];


            var diff = (double)Math.Abs(brightness1 - brightness2)*0.392156;
            return diff < (double)PixelBrightPercentageThreshold;
        }

        static bool IsSquareMatchWithDrift(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
        {
            // int driftRange = squareSize/2;
            int driftRange = 1;

            for (int dy = 0; dy <= driftRange; dy++)
            {
                for (int dx = 0; dx <= driftRange; dx++)
                {
                    if (IsDriftMatch(x, dx, y, dy, bitmapData1, bitmapData2, squareRect))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool IsDriftMatch(int x, int dx, int y, int dy, BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
        {
            return IsSquareMatch(x+dx, y+dy, bitmapData1, bitmapData2, squareRect)|| 
                   IsSquareMatch(x-dx, y-dy, bitmapData1, bitmapData2, squareRect);
        }
        static unsafe bool IsSquareMatch(int x, int y, BitmapData bitmapData1, BitmapData bitmapData2, Rectangle squareRect)
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
            return ((double)differenceCounter / pixelCount) * 100 <= PixelCounterPercentageThreshold;
        }
    }    
}


