namespace ImageComparator.Tests;
using ImageComparator;
public class EntryPointTests
{
    [Test]
    public void TestSettingsDefaule()
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