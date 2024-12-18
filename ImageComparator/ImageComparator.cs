using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;
using ImageComparator.Filters;

namespace ImageComparator
{
    /// <summary>
    /// Performs image comparison operations and tracks the progress of the comparison.
    /// </summary>
    public class Comparator
    {
        /// <summary>
        /// Tracks the progress of the image comparison operation, as a percentage (0-100).
        /// </summary>
        public static int Progress { get; private set; } = 0;

        /// <summary>
        /// Compares two bitmap images, detects differing regions, and outputs a result image.
        /// </summary>
        /// 
        /// <param name="bitmap1">The first bitmap image to compare.</param>
        /// <param name="bitmap2">The second bitmap image to compare.</param>
        /// <param name="bitmapResult">A bitmap where the resulting image with highlighted differences will be drawn.</param>
        ///
        /// <exception cref="ArgumentException">
        /// Thrown if the input images do not have the same dimensions.
        /// </exception>
        ///
        /// <remarks>
        /// The comparison works by dividing the images into square regions of size defined in 
        /// <see cref="Settings.squareSize"/>. It then compares these regions pixel by pixel to 
        /// identify differences. If the differences exceed <see cref="Settings.DiffCount"/>, 
        /// the comparison terminates early.
        /// </remarks>
        ///
        /// <example>
        /// <code>
        /// using Bitmap bitmap1 = new Bitmap("image1.jpg");
        /// using Bitmap bitmap2 = new Bitmap("image2.jpg");
        /// using Bitmap result = new Bitmap(bitmap1.Width, bitmap1.Height);
        /// Comparator.ImageCompare(bitmap1, bitmap2, result);
        /// result.Save("result.jpg");
        /// </code>
        /// </example>
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
                                CombineSquares.AddOrExtend(x, y, x + currentSquareWidth, y + currentSquareHeight);
                                if (CombineSquares.Squares.Count > Settings.DiffCount)
                                {
                                    Progress = 100;
                                    break; 
                                }
                                
                                if (Settings.Debug)
                                {
                                    DrawRectangles.DrawBorder(bitmapResult, squareRect, debug: true);
                                }
                            }

                            processedSquares++;
                            Progress = (int)((processedSquares / (double)totalSquares) * 100);
                        }

                        if (CombineSquares.Squares.Count > Settings.DiffCount)
                        {
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
