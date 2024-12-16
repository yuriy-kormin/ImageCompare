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
        public static List<int> DebugRectColor { get; } = new List<int>(){0,0,0};
        public static bool applyGaussianBlur { get; } = false;
        // public static bool GrayScaleCompare { get; } = false;
        public static int squareSize { get; } = 15;
        public static int PixelCounterPercentageThreshold { get; } = 20;
        public static double GaussianSigma { get; } = 2.0f;
        public static int PixelBrightPercentageThreshold { get; set; } = 10; // overwritten by input theadshold
        public static int DiffCount { get; set; } = -1; // overwritten by input theadshold
    }

    public static class Comparator
    {
        public static int Progress { get; private set; } = 0;
        private static Bitmap Bitmap1 { get; set; }
        private static Bitmap Bitmap2 { get; set; }
        private static Bitmap BitmapResult { get; set; }
        
        public static  void ImageCompare(Bitmap bitmap1, Bitmap bitmap2, Bitmap bitmapResult)
        {
            InputDataValidator.CheckSameDimensions(bitmap1, bitmap2);
            Bitmap1 = bitmap1;
            Bitmap2 = bitmap2;
            BitmapResult = bitmapResult;
            
            int width = bitmap1.Width;
            int height = bitmap1.Height;

            if (Settings.applyGaussianBlur)
            {
                foreach (Bitmap bm in new[] { bitmap1, bitmap2 })
                {
                    GaussBlur.ApplyGaussianBlur(bm, Settings.GaussianSigma);
                }
    
            }
            
            int processedSquares = 0;
            int totalSquares = ((width + Settings.squareSize - 1) / Settings.squareSize) * 
            ((height + Settings.squareSize - 1) / Settings.squareSize);
            
            for (int y = 0; y < height; y += Settings.squareSize)
            {
                for (int x = 0; x < width; x += Settings.squareSize)
                {
                    int currentSquareWidth = Math.Min(Settings.squareSize, width - x);
                    int currentSquareHeight = Math.Min(Settings.squareSize, height - y);
                    
                    Rectangle squareRect = new Rectangle(x, y, currentSquareWidth, currentSquareHeight);
                    
                    ProcessSquare(squareRect);
                    
                    processedSquares++;
                    Progress = (int)((processedSquares / (double)totalSquares) * 100);
                }
            }
            
            foreach (var square in CombineSquares.Squares)
            {
                DrawRectangles.DrawBorder(bitmapResult, square.x1, square.y1, square.x2 - square.x1,
                    square.y2 - square.y1);
            }
        }

        private static void ProcessSquare(Rectangle rect)
        {
            BitmapData? bitmapData1 = null;
            BitmapData? bitmapData2 = null;
            try
            {

                bitmapData1 = BitmapLockBits.Lock(Bitmap1, ImageLockMode.ReadOnly, rect);
                bitmapData2 = BitmapLockBits.Lock(Bitmap2, ImageLockMode.ReadOnly, rect);

                var shiftPX = Settings.DebugRectShiftPX; 
                if (!ComparingMethods.IsSquareMatch(bitmapData1, bitmapData2, rect))
                {
                    if (Settings.Debug)
                    {
                        DrawRectangles.DrawBorder(
                            BitmapResult, 
                            rect.X+shiftPX, rect.Y+shiftPX,
                            rect.Width-shiftPX*2, rect.Height-shiftPX*2, 
                            Settings.DebugRectColor );
                    }
                    CombineSquares.AddOrExtend(rect.X, rect.Y, rect.X+rect.Width, rect.Y+rect.Height);
                }
                
            }
            finally
            {
                if (bitmapData1 != null) { Bitmap1.UnlockBits(bitmapData1);}
                if (bitmapData2 != null) { Bitmap2.UnlockBits(bitmapData2);}
            }
            

        }
            
    }

}


