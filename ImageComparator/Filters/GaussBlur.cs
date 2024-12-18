using System.Drawing;
using System.Drawing.Imaging;
using ImageComparator.Utils;

namespace ImageComparator.Filters
{
    /// <summary>Class to apply Gaussian Blur on a Bitmap image using a Gaussian kernel.</summary>
    public static class GaussBlur
    {
        /// <summary>
        /// Applies a Gaussian blur to the provided Bitmap image.
        /// </summary>
        /// 
        /// <param name="sourceBitmap">The source Bitmap image to blur.</param>
        /// <param name="sigma">The standard deviation (sigma) for the Gaussian distribution.</param>
        public static void ApplyGaussianBlur(Bitmap sourceBitmap, double sigma)
        {
            double[,] kernel = CreateGaussianKernel(sigma, out int kernelSize);
            int offset = kernelSize / 2;

            Rectangle rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            BitmapData? sourceData = null;

            try
            {
                sourceData = BitmapLockBits.Lock(sourceBitmap, ImageLockMode.ReadWrite);
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
                            ApplyKernel(tempBuffer, srcPtr, stride, bytesPerPixel, kernel, kernelSize, x, y, offset);
                        }
                    }
                }
            }
            finally
            {
                if (sourceData != null) { sourceBitmap.UnlockBits(sourceData); }
            }
        }

        /// <summary>
        /// Applies the Gaussian kernel to a specific pixel in the image.
        /// </summary>
        /// 
        /// <param name="tempBuffer">A temporary buffer containing the image pixel data.</param>
        /// <param name="outputPtr">Pointer to the output pixel data.</param>
        /// <param name="stride">The stride (width in bytes) of the image data.</param>
        /// <param name="bytesPerPixel">The number of bytes per pixel in the image format.</param>
        /// <param name="kernel">The Gaussian kernel.</param>
        /// <param name="kernelSize">The size of the Gaussian kernel.</param>
        /// <param name="x">The x-coordinate of the current pixel.</param>
        /// <param name="y">The y-coordinate of the current pixel.</param>
        /// <param name="offset">The offset to center the kernel around the pixel.</param>
        private static unsafe void ApplyKernel(byte[] tempBuffer, byte* outputPtr, int stride, int bytesPerPixel, double[,] kernel, int kernelSize, int x, int y, int offset)
        {
            double[] colorSum = { 0, 0, 0 }; // For R, G, B channels

            // Apply the kernel to the surrounding pixels
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
                }
            }

            // Write the new pixel values back (clamped between 0 and 255)
            int resultIndex = (y * stride) + (x * bytesPerPixel);
            for (int c = 0; c < 3; c++)
            {
                outputPtr[resultIndex + c] = (byte)Math.Min(Math.Max(colorSum[c], 0), 255);
            }
        }

        /// <summary>
        /// Creates a Gaussian kernel matrix based on the provided sigma.
        /// </summary>
        /// 
        /// <param name="sigma">The standard deviation (sigma) for the Gaussian distribution.</param>
        /// <param name="size">Outputs the size of the Gaussian kernel.</param>
        /// <returns>A 2D Gaussian kernel as a double array.</returns>
        private static double[,] CreateGaussianKernel(double sigma, out int size)
        {
            size = (int)(Math.Ceiling(sigma * 3) * 2 + 1);
            double[,] kernel = new double[size, size];
            int offset = size / 2;

            double twoSigmaSquare = 2 * sigma * sigma;
            double sum = 0;

            // Generate kernel values
            for (int y = -offset; y <= offset; y++)
            {
                for (int x = -offset; x <= offset; x++)
                {
                    double value = Math.Exp(-(x * x + y * y) / twoSigmaSquare) / (Math.PI * twoSigmaSquare);
                    kernel[y + offset, x + offset] = value;
                    sum += value;
                }
            }

            // Normalize kernel values
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    kernel[y, x] /= sum;
                }
            }

            return kernel;
        }
    }
}
