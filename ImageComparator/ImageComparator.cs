using ImageComparator.Filters;

namespace ImageComparator;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;



public class ImageComparator
{
    static void Main(string[] args)
    {
        // Input and output file paths
        // string inputFile = "input.jpg";
        string imagePath1 = "1-1.jpg";
        string imagePath2 = "2-2.jpg";

        string outputFile = "output2.jpg";
        var image1 = new Bitmap(imagePath1);
        var image2 = new Bitmap(imagePath2);
        
        var c_image1 = IncreaseContrast.Process(image1);
        var c_image2 = IncreaseContrast.Process(image2);
        
        c_image1.Save($"c_{imagePath1}", ImageFormat.Jpeg);
        c_image2.Save($"c_{imagePath2}", ImageFormat.Jpeg);
        
        var g_image1 = GrayScale.Process(image1);
        var g_image2 = GrayScale.Process(image2);
        
        g_image1.Save($"g_{imagePath1}", ImageFormat.Jpeg);
        g_image2.Save($"g_{imagePath2}", ImageFormat.Jpeg);


        //
        // Bitmap result = CompareAndReplacePixels(image1,image1, image2,50);
        // result.Save($"o_{outputFile}",ImageFormat.Jpeg);
        
        Bitmap c_result = CompareAndReplacePixels(image1,g_image1, g_image2,50);
        c_result.Save($"c_{outputFile}",ImageFormat.Jpeg);
        
        Bitmap g_result = CompareAndReplacePixels(image1,g_image1, g_image2,50);
        g_result.Save($"c_g_{outputFile}",ImageFormat.Jpeg);


        // //
        // AppContext.TryGetSwitch("System.Drawing.EnableUnixSupport", out bool isSDCEnabled);
        // Console.WriteLine(isSDCEnabled);

        // AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
        //
        // AppContext.TryGetSwitch("System.Drawing.EnableUnixSupport", out bool isSDCEnabled);
        // Console.WriteLine(isSDCEnabled);
    }

    //     try
    //     {
    //         // Load the input image
    //         using (Bitmap bitmap = new Bitmap(inputFile))
    //         {
    //             // Invert colors using LockBits
    //             InvertColors(bitmap);
    //             
    //             // Save the manipulated image
    //             bitmap.Save(outputFile, ImageFormat.Jpeg);
    //         }
    //     
    //         Console.WriteLine("Image processed and saved to " + outputFile);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Error: " + ex.Message);
    //     }
    // }
    
    static Bitmap CompareAndReplacePixels(Bitmap image0, Bitmap image1, Bitmap image2,int threshold)
    {
        if (image1.Width != image2.Width || image1.Height != image2.Height)
            throw new ArgumentException("Images must have the same dimensions.");

        Bitmap result = new Bitmap(image1.Width, image1.Height, PixelFormat.Format24bppRgb);
        Rectangle rect = new Rectangle(0, 0, image1.Width, image1.Height);

        // Lock bits for all three images
        BitmapData data0 = image0.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData data1 = image1.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData data2 = image2.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        BitmapData resultData = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

        int bytes = Math.Abs(data1.Stride) * image1.Height;

        // Allocate managed arrays for pixel data
        byte[] buffer0 = new byte[bytes];
        byte[] buffer1 = new byte[bytes];
        byte[] buffer2 = new byte[bytes];
        byte[] resultBuffer = new byte[bytes];

        // Copy data to managed arrays
        Marshal.Copy(data0.Scan0, buffer0, 0, bytes);
        Marshal.Copy(data1.Scan0, buffer1, 0, bytes);
        Marshal.Copy(data2.Scan0, buffer2, 0, bytes);

        // Process pixel data
        int bytesPerPixel = 3; // 24 bits per pixel = 3 bytes
        for (int i = 0; i < bytes; i += bytesPerPixel)
        {
            byte b0 = buffer0[i];
            byte g0 = buffer0[i + 1];
            byte r0 = buffer0[i + 2];
            
            byte b1 = buffer1[i];
            byte g1 = buffer1[i + 1];
            byte r1 = buffer1[i + 2];

            byte b2 = buffer2[i];
            byte g2 = buffer2[i + 1];
            byte r2 = buffer2[i + 2];
            
            int diffR = r1 - r2;
            int diffG = g1 - g2;
            int diffB = b1 - b2;
            int distance = (int)Math.Sqrt(diffR * diffR + diffG * diffG + diffB * diffB);

            if (distance > threshold)
            
            {
                // Replace with pixel from image2
                resultBuffer[i] = 0;
                resultBuffer[i + 1] = 0;
                resultBuffer[i + 2] = 255;
            }
            else
            {
                // Keep original pixel from image1
                resultBuffer[i] = b0;
                resultBuffer[i + 1] = g0;
                resultBuffer[i + 2] = r0;
            }
        }

        Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);

        // Unlock bits
        image0.UnlockBits(data0);
        image1.UnlockBits(data1);
        image2.UnlockBits(data2);
        result.UnlockBits(resultData);

        return result;
    }

    static void InvertColors(Bitmap bitmap)
    {
        // Lock the bitmap's bits
        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
        
        try
        {
            // Get the address of the first pixel
            IntPtr ptr = bitmapData.Scan0;
        
            // Calculate the number of bytes in the image
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * bitmap.Height;
        
            // Create a byte array to hold the pixel data
            byte[] pixels = new byte[byteCount];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, byteCount);
        
            // Iterate through each pixel
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int index = y * bitmapData.Stride + x * bytesPerPixel;
        
                    // Invert the color values (assumes 24bpp or 32bpp image format)
                    pixels[index] = (byte)(255 - pixels[index]);       // Blue
                    pixels[index + 1] = (byte)(255 - pixels[index + 1]); // Green
                    pixels[index + 2] = (byte)(255 - pixels[index + 2]); // Red
                }
            }
        
            // Copy the modified pixel data back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, byteCount);
        }
        finally
        {
            // Unlock the bits
            bitmap.UnlockBits(bitmapData);
        }
    }
}