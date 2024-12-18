using System.Drawing;
using ImageComparator.Filters;
using ImageComparator.Tests;
namespace ImageComparator.Tests.FilterTests;

public class GaussianBlurTests
{
    private Bitmap _bitmap { get; }=BitmapFixture.CreateSolidColorBitmap(20, 20, Color.Blue);
    
    [Test]
    public void ApplyGaussianBlur_ShouldBlurImage_WhenValidInput()
    {
        GaussBlur.ApplyGaussianBlur(_bitmap, 2.0f);

        var originalColor = Color.Blue;
        var blurredColor = BitmapFixture.GetPixelColor(_bitmap, _bitmap.Width / 2, _bitmap.Height / 2);
        Assert.That(originalColor, Is.Not.EqualTo(blurredColor));
    }
}