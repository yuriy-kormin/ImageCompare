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
            Assert.AreEqual(GetBitmapHash(_bitmapResult), GetBitmapHash(_bitmap2));
            
            BitmapFixture.DrawFillRectangle(_bitmap2,10, 10, 30, 30, Color.Black);
            Comparator.ImageCompare(_bitmap1, _bitmap2, _bitmapResult);
            
            Assert.AreNotEqual(GetBitmapHash(_bitmapResult),GetBitmapHash(_bitmap1));
        }

        [Test]
        public void ImageCompare_Thredshold()
        {
            // Assert.AreEqual(GetBitmapHash(_bitmapResult), GetBitmapHash(_bitmap1));
            // Assert.AreEqual(GetBitmapHash(_bitmapResult), GetBitmapHash(_bitmap2));
            //
            BitmapFixture.DrawFillRectangle(_bitmap2,10, 10, 30, 30, Color.Beige);
            Comparator.ImageCompare(_bitmap1, _bitmap2, _bitmapResult);
            Assert.AreEqual(GetBitmapHash(_bitmapResult),GetBitmapHash(_bitmap1));
            
            Settings.PixelBrightPercentageThreshold = 1;
            Comparator.ImageCompare(_bitmap1, _bitmap2, _bitmapResult);
            Assert.AreNotEqual(GetBitmapHash(_bitmapResult),GetBitmapHash(_bitmap1));
        }

        [Test]
        public void ImageCompare_Percentage()
        {
            Assert.AreEqual(Comparator.Progress,0);
            Comparator.ImageCompare(_bitmap1, _bitmap2, _bitmapResult);
            Assert.AreEqual(Comparator.Progress,100);
        }
        
        
        [Test]
        public void TestSettingsDefault()
        {
            Assert.That(Settings.squareSize, Is.EqualTo(15));
            Assert.That(Settings.PixelCounterPercentageThreshold, Is.EqualTo(20));
            Assert.That(Settings.GaussianSigma, Is.EqualTo(2.0f));

            Assert.That(Settings.PixelBrightPercentageThreshold, Is.EqualTo(10));
            Assert.That(Settings.DiffCount, Is.EqualTo(-1));
        

            Settings.PixelBrightPercentageThreshold = 44;
            Settings.DiffCount = 44;
            Assert.That(Settings.PixelBrightPercentageThreshold, Is.EqualTo(44));
            Assert.That(Settings.DiffCount, Is.EqualTo(44));

            Assert.That(Settings.squareSize, Is.EqualTo(15));
            Assert.That(Settings.PixelCounterPercentageThreshold, Is.EqualTo(20));
            Assert.That(Settings.GaussianSigma, Is.EqualTo(2.0f));

        }
    }
}
