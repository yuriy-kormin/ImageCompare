using System.Drawing;
using System.Drawing.Imaging;

namespace ImageComparator.Tests
{
    [TestFixture]
    public class BitmapFixture
    {
        public static Bitmap CreateSolidColorBitmap(int width, int height, Color color)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            try
            {
                unsafe
                {
                    byte* ptr = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            ptr[0] = color.B; // Blue
                            ptr[1] = color.G; // Green
                            ptr[2] = color.R; // Red
                            ptr += 3;
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            return bitmap;
        }
        public static unsafe Color GetPixelColor(Bitmap bitmap, int x, int y)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            try
            {
                byte* ptr = (byte*)bitmapData.Scan0 + y * bitmapData.Stride + x * 3;
                byte b = ptr[0];
                byte g = ptr[1];
                byte r = ptr[2];
                return Color.FromArgb(r,g,b);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

    }    
};

