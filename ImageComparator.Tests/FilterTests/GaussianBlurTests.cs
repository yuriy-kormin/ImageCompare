using System.Drawing;
using ImageComparator.Filters;
using ImageComparator.Tests;
namespace ImageComparator.Tests.FilterTests;

public class GaussianBlurTests
{
    private Bitmap _bitmap { get; }=BitmapFixture.CreateSolidColorBitmap(10, 10, Color.Blue);
    
    [Test]
    public void ApplyGaussianBlur_ShouldBlurImage_WhenValidInput()
    {
        using var bitmap = BitmapFixture.CreateSolidColorBitmap(20, 20, Color.Blue);

        GaussBlur.ApplyGaussianBlur(bitmap, 2.0f);

        // Assert
        var originalColor = Color.Blue;
        var blurredColor = BitmapFixture.GetPixelColor(bitmap, bitmap.Width / 2, bitmap.Height / 2);
        Assert.AreNotEqual(originalColor, blurredColor, "The Gaussian blur should modify the center pixel.");
    }
}