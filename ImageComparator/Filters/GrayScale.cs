using System.Drawing;
using System.Net.Mime;
using System.Runtime.InteropServices;
using ImageComparator.utils;

namespace ImageComparator.Filters;



public class GrayScale: IConverter
{
    public static Bitmap Process(Bitmap image)
    {
        Bitmap grayscaleImage = new Bitmap(image.Width, image.Height);

        // Lock the bits of the source image for fast access
        BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData resultData = grayscaleImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

        // Get the pointers to the image data
        IntPtr ptr = imageData.Scan0;
        IntPtr resultPtr = resultData.Scan0;

        // Get the bytes per pixel
        int bytesPerPixel = MediaTypeNames.Image.GetPixelFormatSize(image.PixelFormat) / 8;
        int byteCount = imageData.Stride * image.Height;

        byte[] imageBytes = new byte[byteCount];
        byte[] resultBytes = new byte[byteCount];

        // Copy the image data to a byte array
        Marshal.Copy(ptr, imageBytes, 0, byteCount);

        // Process each pixel in the image
        for (int i = 0; i < imageBytes.Length; i += bytesPerPixel)
        {
            byte b = imageBytes[i];
            byte g = imageBytes[i + 1];
            byte r = imageBytes[i + 2];

            // Calculate the grayscale value using the luminosity method
            byte grayValue = (byte)(0.3 * r + 0.59 * g + 0.11 * b);

            // Set the grayscale value for all three channels (R, G, B)
            resultBytes[i] = grayValue;
            resultBytes[i + 1] = grayValue;
            resultBytes[i + 2] = grayValue;
        }

        // Copy the modified byte array back to the result image
        Marshal.Copy(resultBytes, 0, resultPtr, byteCount);

        // Unlock the bits to apply the changes
        image.UnlockBits(imageData);
        grayscaleImage.UnlockBits(resultData);

        return grayscaleImage;        
    }
}