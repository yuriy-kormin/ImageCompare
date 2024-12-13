namespace ImageComparator.Tests;
using ImageComparator;
public class Tests
{
    // [SetUp]
    // public void Setup()
    // {
    // }

    [Test]
    public void TestSettingsDefaule()
    {
        Assert.AreEqual(Settings.squareSize, 15);
        Assert.AreEqual(Settings.PixelCounterPercentageThreshold, 20);
        Assert.AreEqual(Settings.GaussianSigma, 2.0f);
        
        Assert.AreEqual(Settings.PixelBrightPercentageThreshold, 10);
        Assert.AreEqual(Settings.DiffCount, -1);

        Settings.PixelBrightPercentageThreshold = 44;
        Settings.DiffCount = 44;
        Assert.AreEqual(Settings.PixelBrightPercentageThreshold, 44);
        Assert.AreEqual(Settings.DiffCount, 44);
        
        Assert.AreEqual(Settings.squareSize, 15);
        Assert.AreEqual(Settings.PixelCounterPercentageThreshold, 20);
        Assert.AreEqual(Settings.GaussianSigma, 2.0f);
    }
}