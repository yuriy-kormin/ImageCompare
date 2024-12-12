using System;
using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;

namespace ImageComparator.Filters
{
    public static class GaussBlur
    {
        public static void ApplyGaussianBlur(Bitmap sourceBitmap, double sigma)
        {
            // Generate Gaussian kernel
            int[,] kernel = CreateGaussianKernel(sigma, out int kernelSize);
            int offset = kernelSize / 2;

            // Lock bitmap data for in-place modification
            Rectangle rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            BitmapData? sourceData = null;

            try
            {
                sourceData = BitmapLockBits.Lock(sourceBitmap, ImageLockMode.ReadWrite);
                // sourceData = sourceBitmap.LockBits(rect, ImageLockMode.ReadWrite, sourceBitmap.PixelFormat);
                int bytesPerPixel = Image.GetPixelFormatSize(sourceBitmap.PixelFormat) / 8;
                int stride = sourceData.Stride;
                int width = sourceBitmap.Width;
                int height = sourceBitmap.Height;

                unsafe
                {
                    byte* srcPtr = (byte*)sourceData.Scan0.ToPointer();
                    byte[] tempBuffer = new byte[stride * height];
                    System.Runtime.InteropServices.Marshal.Copy((IntPtr)srcPtr, tempBuffer, 0, tempBuffer.Length);

                    for (int y = offset; y < height - offset; y++)
                    {
                        for (int x = offset; x < width - offset; x++)
                        {
                            ApplyKernel(tempBuffer, srcPtr, stride, bytesPerPixel, width, height, kernel, kernelSize, x, y, offset);
                        }
                    }
                }
            }
            finally
            {
                if (sourceData != null) {sourceBitmap.UnlockBits(sourceData);}
            }
        }

        private static unsafe void ApplyKernel(byte[] tempBuffer, byte* outputPtr, int stride, int bytesPerPixel, int width, int height, int[,] kernel, int kernelSize, int x, int y, int offset)
        {
            double[] colorSum = { 0, 0, 0 }; // For R, G, B channels
            int kernelSum = 0;

            for (int ky = -offset; ky <= offset; ky++)
            {
                for (int kx = -offset; kx <= offset; kx++)
                {
                    int pixelX = x + kx;
                    int pixelY = y + ky;

                    int pixelIndex = (pixelY * stride) + (pixelX * bytesPerPixel);

                    for (int c = 0; c < 3; c++) // Iterate over color channels
                    {
                        colorSum[c] += tempBuffer[pixelIndex + c] * kernel[ky + offset, kx + offset];
                    }

                    kernelSum += kernel[ky + offset, kx + offset];
                }
            }

            int resultIndex = (y * stride) + (x * bytesPerPixel);
            for (int c = 0; c < 3; c++)
            {
                outputPtr[resultIndex + c] = (byte)(colorSum[c] / kernelSum);
            }
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
                    double value = Math.Exp(-(x * x + y * y) / twoSigmaSquare) / sigmaRoot;
                    kernel[y + offset, x + offset] = (int)(value * 1000);
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
