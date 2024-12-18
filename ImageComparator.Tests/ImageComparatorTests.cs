using System.Drawing;
using ImageComparator;
using ImageComparator.Tests;

namespace YourNamespace.Tests
{
    
    public class ComparatorTests
    {
        private Bitmap _bitmap1;
        private Bitmap _bitmap2;
        private Bitmap _bitmapResult;


        [SetUp]
        public void Setup()
        {
            _bitmap1 = BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
            _bitmap2 = BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
            _bitmapResult=BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
        }


        [Test]
        public void TestSettingsDefault()
        {
            Settings.PixelBrightPercentageThreshold = 44;
            Settings.DiffCount = 44;
            Assert.That(Settings.PixelBrightPercentageThreshold, Is.EqualTo(44));
            Assert.That(Settings.DiffCount, Is.EqualTo(44));

            Assert.That(Settings.squareSize, Is.EqualTo(15));
            Assert.That(Settings.PixelCounterPercentageThreshold, Is.EqualTo(20));
            Assert.That(Settings.GaussianSigma, Is.EqualTo(2.0f));

        }

        [Test]
        public void ImageCompare_GrenCase()
        {
            Assert.That(BitmapFixture.IsEqualByHash(_bitmap1, _bitmapResult), Is.True);
            Assert.That(BitmapFixture.IsEqualByHash(_bitmap2, _bitmapResult), Is.True);

            BitmapFixture.DrawFillRectangle(_bitmap2, 10, 10, 30, 30, Color.Black);
            Comparator.ImageCompare(_bitmap1, _bitmap2, _bitmapResult);

            Assert.That(BitmapFixture.IsEqualByHash(_bitmap1, _bitmapResult), Is.False);
        }
    }
}
