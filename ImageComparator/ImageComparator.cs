using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;


public class ImageComparator
{
    private static int squareSize { get; } = 15;
    private static int PixelCounterPercentageThreshold { get; } = 20;
    private static int PixelBrightPercentageThreshold { get; } = 10;

    public static List<(int x1, int y1, int x2, int y2)> Squares { get; private set; } =
        new List<(int, int, int, int)>();

    public static List<double> bright {get; private set; } = new List<double>();

    public static void ImageCompare()
    {
        string imagePath1 = "1-1.jpg";
        string imagePath2 = "2-2.jpg";
        string outputFile = "output.jpg";

        Bitmap bitmap1 = new Bitmap(imagePath1);
        Bitmap bitmap2 = new Bitmap(imagePath2);
        Bitmap bitmapResult = new Bitmap(imagePath2); // Create result bitmap with the same dimensions
        
        double sigma = 2.0;
        Bitmap bitmap1_b = GaussBlur.ApplyGaussianBlur(bitmap1, sigma);
        Bitmap bitmap2_b = GaussBlur.ApplyGaussianBlur(bitmap2, sigma);
        bitmap2_b.Save($"2{outputFile}");
        
        Rectangle fullRect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
        BitmapData bitmapData1 = bitmap1_b.LockBits(fullRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData bitmapData2 = bitmap2_b.LockBits(fullRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData bitmapResultData = bitmapResult.LockBits(fullRect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        try
        {
            int width = bitmap1.Width;
            int height = bitmap1.Height;

            unsafe
            {
                byte* resultPtr = (byte*)bitmapResultData.Scan0;

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
                            Draw.DrawRedBorder(resultPtr, x, y, currentSquareWidth, currentSquareHeight, bitmapResultData.Stride, 3);
                        }
                    }
                }
            }
        }
        finally
        {
            bitmap1_b.UnlockBits(bitmapData1);
            bitmap2_b.UnlockBits(bitmapData2);
            bitmapResult.UnlockBits(bitmapResultData);
        }

        bitmapResult.Save(outputFile);
        Console.WriteLine($"Output saved to {outputFile}");
        float max = float.MinValue;
        float min = float.MaxValue;
        foreach (float number in bright)
        {
            if (number > max) max = number;
            if (number < min) min = number;
        }
        
        Console.WriteLine($"min value={min}, max value={max}");
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
        // Console.WriteLine($"Pixel count: {((double)differenceCounter / pixelCount) * 100}");
        return ((double)differenceCounter / pixelCount) * 100 <= PixelCounterPercentageThreshold;
    }
}
