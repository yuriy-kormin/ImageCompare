using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ImageComparator.utils;

namespace ImageComparator.Filters;

public class IncreaseContrast: IConverter
{
    private const float Factor = 2.0f; 
    public static Bitmap Process(Bitmap image)
    {
        Bitmap contrastedImage = new Bitmap(image.Width, image.Height);

        // Lock the bits of the source image for fast access
        BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData resultData = contrastedImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);


        // Get the pointers to the image data
        IntPtr ptr = imageData.Scan0;
        IntPtr resultPtr = resultData.Scan0;

        // Get the bytes per pixel
        int bytesPerPixel = Image.GetPixelFormatSize(image.PixelFormat) / 8;
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

            // Apply the maximum contrast adjustment to each RGB channel
            r = (byte)Math.Min(Math.Max(Factor * (r - 128) + 128, 0), 255);
            g = (byte)Math.Min(Math.Max(Factor * (g - 128) + 128, 0), 255);
            b = (byte)Math.Min(Math.Max(Factor * (b - 128) + 128, 0), 255);

            resultBytes[i] = b;
            resultBytes[i + 1] = g;
            resultBytes[i + 2] = r;
        }

        // Copy the modified byte array back to the result image
        Marshal.Copy(resultBytes, 0, resultPtr, byteCount);

        // Unlock the bits to apply the changes
        image.UnlockBits(imageData);
        contrastedImage.UnlockBits(resultData);

        return contrastedImage;       
    }
}