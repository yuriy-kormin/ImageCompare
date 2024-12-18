using System.Drawing;
using ImageComparator;
using ImageComparator.Tests;

namespace YourNamespace.Tests;

public class BitmapThresholdTest
{
    
    [Test]
    public void ImageCompare_Thredshold()
    {
        var Bm1=BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
        var Bm2=BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
        var Bm3=BitmapFixture.CreateSolidColorBitmap(100, 100, Color.White);
        Assert.That(BitmapFixture.IsEqualByHash(Bm1, Bm3), Is.True);
        Assert.That(BitmapFixture.IsEqualByHash(Bm2, Bm3), Is.True);
        
        BitmapFixture.DrawFillRectangle(Bm2, 10, 10, 30, 30, Color.Azure);
        Settings.PixelBrightPercentageThreshold = 10;
        Comparator.ImageCompare(Bm1, Bm2, Bm3);

        Assert.That(BitmapFixture.IsEqualByHash(Bm1, Bm3), Is.True);
        
        Settings.PixelBrightPercentageThreshold = 0;
        Comparator.ImageCompare(Bm1, Bm2, Bm3);
        Assert.That(BitmapFixture.IsEqualByHash(Bm1, Bm3), Is.False);
    }
}