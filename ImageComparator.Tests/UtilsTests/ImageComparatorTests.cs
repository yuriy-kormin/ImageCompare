using System.Drawing;
using System.Security.Cryptography;
using ImageComparator;
using ImageComparator.Tests;
using ImageComparator.Utils;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class ComparatorTests
    {
        private Bitmap _bitmap1 = BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
        private Bitmap _bitmap2 = BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
        private Bitmap _bitmapResult=BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);

        [SetUp]
        public void SetUp()
        {
            BitmapFixture.DrawFillRectangle(_bitmap2,10, 10, 30, 30, Color.Black);
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

        [Test]
        public void ImageCompare_GrenCase()
        {
            Assert.AreEqual(GetBitmapHash(_bitmapResult), GetBitmapHash(_bitmap1));
            Assert.AreNotEqual(GetBitmapHash(_bitmapResult), GetBitmapHash(_bitmap2));
            Comparator.ImageCompare(_bitmap1, _bitmap2, _bitmapResult);
            Assert.AreNotEqual(GetBitmapHash(_bitmapResult),GetBitmapHash(_bitmap1));
        }
        
    }
}
