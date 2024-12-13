using System.Drawing;
using ImageComparator.Utils;

public class InputDataValidatorTests
{
    [Test]
    public void CheckSameDimensions_DifferentWidth_ThrowsException()
    {
        // Arrange
        var bitmap1 = new Bitmap(100, 100); // 100x100 dimensions
        var bitmap2 = new Bitmap(200, 100); // 200x100 dimensions

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => InputDataValidator.CheckSameDimensions(bitmap1, bitmap2));
        Assert.AreEqual("Bitmaps are different.", ex.Message); // Check if the exception message is correct
    }

    [Test]
    public void CheckSameDimensions_DifferentHeight_ThrowsException()
    {
        // Arrange
        var bitmap1 = new Bitmap(100, 100); // 100x100 dimensions
        var bitmap2 = new Bitmap(100, 200); // 100x200 dimensions

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => InputDataValidator.CheckSameDimensions(bitmap1, bitmap2));
        Assert.AreEqual("Bitmaps are different.", ex.Message); // Check if the exception message is correct
    }

    [Test]
    public void CheckSameDimensions_SameDimensions_DoesNotThrowException()
    {
        // Arrange
        var bitmap1 = new Bitmap(100, 100); // 100x100 dimensions
        var bitmap2 = new Bitmap(100, 100); // 100x100 dimensions

        // Act & Assert
        Assert.DoesNotThrow(() => InputDataValidator.CheckSameDimensions(bitmap1, bitmap2));
    }
}
