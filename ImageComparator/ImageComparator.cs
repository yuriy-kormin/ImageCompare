using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;
using ImageComparator.Filters;

namespace ImageComparator
{
    public class Settings
    {
        public static bool Debug { get; } = true;
        public static int DebugRectShiftPX { get; } = 3;
        public static Color DebugRectColor { get; } = Color.Black;
        public static Color RectColor { get; } = Color.Red;
        
        public static bool applyGaussianBlur { get; } = false;
        // public static bool GrayScaleCompare { get; } = false;
        public static int squareSize { get; } = 15;
        public static int PixelCounterPercentageThreshold { get; } = 20;
        public static double GaussianSigma { get; } = 2.0f;
        public static int PixelBrightPercentageThreshold { get; set; } = 10; // overwritten by input theadshold
        public static int DiffCount { get; set; } = -1; // overwritten by input theadshold
    }

    public class Comparator
    {
        public static int Progress { get; private set; } = 0;
        public static void ImageCompare(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmapResult)
        {
            InputDataValidator.CheckSameDimensions(bitmap1, bitmap2);
            
            int width = bitmap1.Width;
            int height = bitmap1.Height;

            if (Settings.applyGaussianBlur)
            {
                foreach (Bitmap bm in new[] { bitmap1, bitmap2 })
                {
                    GaussBlur.ApplyGaussianBlur(bm, Settings.GaussianSigma);
                }
    
            }
            
            BitmapData? bitmapData1 = null;
            BitmapData? bitmapData2 = null;

            int processedSquares = 0;
            int totalSquares = ((width + Settings.squareSize - 1) / Settings.squareSize) * 
                               ((height + Settings.squareSize - 1) / Settings.squareSize);

            if (Settings.DiffCount == -1)
            {
                Settings.DiffCount = totalSquares;
                // Max number of diffcount can be found is a total squares. Skip check if diffcount==-1...
            }
            
            try
            {
                
                bitmapData1 = BitmapLockBits.Lock(bitmap1, ImageLockMode.ReadOnly);
                bitmapData2 = BitmapLockBits.Lock(bitmap2, ImageLockMode.ReadOnly);

                unsafe
                {
                    for (int y = 0; y < height; y += Settings.squareSize)
                    {
                        for (int x = 0; x < width; x += Settings.squareSize)
                        {
                            int currentSquareWidth = Math.Min(Settings.squareSize, width - x);
                            int currentSquareHeight = Math.Min(Settings.squareSize, height - y);
                            Rectangle squareRect = new Rectangle(x, y, currentSquareWidth, currentSquareHeight);

                            if (!ComparingMethods.IsSquareMatch(x, y, bitmapData1, bitmapData2, squareRect))
                            {
                                if (Settings.Debug)
                                {
                                    DrawRectangles.DrawBorder(bitmapResult, squareRect,debug:true);
                                }
                                CombineSquares.AddOrExtend(x, y, x + currentSquareWidth, y + currentSquareHeight);
                            }

                            processedSquares++;
                            Progress = (int)((processedSquares / (double)totalSquares) * 100);
                            if (CombineSquares.Squares.Count > Settings.DiffCount)
                            {
                                break; 
                            }
                        }
                        if (CombineSquares.Squares.Count > Settings.DiffCount)
                        {
                            Progress = 100;
                            break;
                        }
                    }

                    var squares = CombineSquares.Squares.GetRange(0,
                        Math.Min(Settings.DiffCount, CombineSquares.Squares.Count));
                    foreach (var square in squares)
                    {
                        Rectangle squareRect = new Rectangle(square.x1, square.y1, square.x2 - square.x1,
                            square.y2 - square.y1);
                        DrawRectangles.DrawBorder(bitmapResult, squareRect);
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

            }
        }
    }
}


