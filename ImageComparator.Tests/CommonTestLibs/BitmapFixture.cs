using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using ImageComparator.Utils;

namespace ImageComparator.Tests
{

    [TestFixture]
    public class BitmapFixture
    {
        public static Bitmap CreateSolidColorBitmap(int width, int height, Color color)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

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
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* ptr = (byte*)bitmapData.Scan0 + y * bitmapData.Stride + x * 3;
                byte b = ptr[0];
                byte g = ptr[1];
                byte r = ptr[2];
                return Color.FromArgb(r, g, b);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static void DrawFillRectangle(Bitmap bitmap, int x, int y, int width, int height, Color color)
        {
            var rect = new Rectangle(x, y, width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
        }

        public static string GetBitmapHash(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bitmapBytes = ms.ToArray();

                // Generate the MD5 hash
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashBytes = md5.ComputeHash(bitmapBytes);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }

        public static bool IsEqualByHash(Bitmap bitmap1, Bitmap bitmap2)
        {
            return (GetBitmapHash(bitmap1) == GetBitmapHash(bitmap2));
        }
    }

};

