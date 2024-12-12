using System.Drawing;
using System.Drawing.Imaging;

namespace ImageComparator.Filters
{
    public class GaussBlur
{
    public static Bitmap ApplyGaussianBlur(Bitmap sourceBitmap, double sigma)
    {
        int[,] kernel = CreateGaussianKernel(sigma, out int kernelSize);
        int offset = kernelSize / 2;

        Bitmap blurredBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
        Rectangle rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);

        BitmapData sourceData = sourceBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData resultData = blurredBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

        int bytesPerPixel = Image.GetPixelFormatSize(PixelFormat.Format24bppRgb) / 8;
        int stride = sourceData.Stride;

        unsafe
        {
            byte* srcPtr = (byte*)sourceData.Scan0.ToPointer();
            byte* dstPtr = (byte*)resultData.Scan0.ToPointer();

            for (int y = offset; y < sourceBitmap.Height - offset; y++)
            {
                for (int x = offset; x < sourceBitmap.Width - offset; x++)
                {
                    double[] colorSum = { 0, 0, 0 };
                    int kernelSum = 0;

                    for (int ky = -offset; ky <= offset; ky++)
                    {
                        for (int kx = -offset; kx <= offset; kx++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;

                            byte* pixel = srcPtr + (pixelY * stride) + (pixelX * bytesPerPixel);

                            for (int c = 0; c < 3; c++) // For R, G, B channels
                            {
                                colorSum[c] += pixel[c] * kernel[ky + offset, kx + offset];
                            }

                            kernelSum += kernel[ky + offset, kx + offset];
                        }
                    }

                    byte* resultPixel = dstPtr + (y * stride) + (x * bytesPerPixel);
                    for (int c = 0; c < 3; c++)
                    {
                        resultPixel[c] = (byte)(colorSum[c] / kernelSum);
                    }
                }
            }
        }

        sourceBitmap.UnlockBits(sourceData);
        blurredBitmap.UnlockBits(resultData);

        return blurredBitmap;
    }

    private static int[,] CreateGaussianKernel(double sigma, out int size)
    {
        size = (int)(Math.Ceiling(sigma * 3) * 2 + 1);
        int[,] kernel = new int[size, size];
        int offset = size / 2;

        double twoSigmaSquare = 2 * sigma * sigma;
        double sigmaRoot = Math.Sqrt(twoSigmaSquare * Math.PI);
        double sum = 0;

        for (int y = -offset; y <= offset; y++)
        {
            for (int x = -offset; x <= offset; x++)
            {
                kernel[y + offset, x + offset] = (int)(Math.Exp(-(x * x + y * y) / twoSigmaSquare) / sigmaRoot * 1000);
                sum += kernel[y + offset, x + offset];
            }
        }

        // Normalize kernel
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                kernel[y, x] = (int)(kernel[y, x] / sum * 1000);
            }
        }

        return kernel;
    }
}
    
}
