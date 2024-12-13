using ImageComparator.Utils;

namespace ImageComparator.Tests.UtilsTests;


public class DrawRectanglesTests
{
    private unsafe byte[] CreateImage(int width, int height, int bytesPerPixel)
    {
        // Allocate a byte array to simulate the pixel data (RGBA format)
        return new byte[width * height * bytesPerPixel];
    }

    [Test]
    public unsafe void DrawBorder_DefaultColor_DrawsBlueBorder()
    {
        // Arrange
        int width = 5, height = 5, stride = 5, bytesPerPixel = 3; // RGB
        byte[] image = CreateImage(width, height, bytesPerPixel);
        fixed (byte* resPtr = image)
        {
            // Act
            DrawRectangles.DrawBorder(resPtr, 1, 1, 3, 3, stride, bytesPerPixel);

            // Assert
            // Check the border pixels (expected blue color)
            Assert.AreEqual(0, image[(1 * stride + 1) * bytesPerPixel]); // Top-left
            Assert.AreEqual(0, image[(1 * stride + 3) * bytesPerPixel]); // Top-right
            Assert.AreEqual(0, image[((3 + 1) * stride + 1) * bytesPerPixel]); // Bottom-left
            Assert.AreEqual(0, image[((3 + 1) * stride + 3) * bytesPerPixel]); // Bottom-right
            Assert.AreEqual(255, image[(1 * stride + 1) * bytesPerPixel + 2]); // Blue channel for top-left
        }
    }

    [Test]
    public unsafe void DrawBorder_CustomColor_DrawsCorrectColor()
    {
        // Arrange
        int width = 5, height = 5, stride = 5, bytesPerPixel = 3; // RGB
        byte[] image = CreateImage(width, height, bytesPerPixel);
        List<int> customColor = new List<int> { 255, 0, 0 }; // Red color
        fixed (byte* resPtr = image)
        {
            // Act
            DrawRectangles.DrawBorder(resPtr, 1, 1, 3, 3, stride, bytesPerPixel, customColor);

            // Assert
            // Check the border pixels (expected red color)
            Assert.AreEqual(255, image[(1 * stride + 1) * bytesPerPixel]); // Top-left red
            Assert.AreEqual(0, image[(1 * stride + 1) * bytesPerPixel + 1]); // Green channel
            Assert.AreEqual(0, image[(1 * stride + 1) * bytesPerPixel + 2]); // Blue channel
        }
    }

    [Test]
    public unsafe void DrawBorder_SinglePixelRectangle_DrawsOnePixel()
    {
        // Arrange
        int width = 1, height = 1, stride = 1, bytesPerPixel = 3; // RGB
        byte[] image = CreateImage(width, height, bytesPerPixel);
        fixed (byte* resPtr = image)
        {
            // Act
            DrawRectangles.DrawBorder(resPtr, 0, 0, 1, 1, stride, bytesPerPixel);

            // Assert
            // Check if the single pixel is colored blue (default)
            Assert.AreEqual(0, image[0]); // Red channel
            Assert.AreEqual(0, image[1]); // Green channel
            Assert.AreEqual(255, image[2]); // Blue channel
        }
    }

    [Test]
    public unsafe void DrawBorder_HandlesEdgeCorrectly()
    {
        // Arrange
        int width = 7, height = 7, stride = 7, bytesPerPixel = 3; // RGB
        byte[] image = CreateImage(width, height, bytesPerPixel);
        fixed (byte* resPtr = image)
        {
            // Act
            DrawRectangles.DrawBorder(resPtr, 0, 0, 7, 7, stride, bytesPerPixel);

            // Assert
            // Check all the border pixels (top, bottom, left, right)
            for (int i = 0; i < 7; i++)
            {
                Assert.AreEqual(0, image[(0 * stride + i) * bytesPerPixel]); // Top row
                Assert.AreEqual(0, image[((6) * stride + i) * bytesPerPixel]); // Bottom row
                Assert.AreEqual(0, image[(i * stride + 0) * bytesPerPixel]); // Left column
                Assert.AreEqual(0, image[(i * stride + 6) * bytesPerPixel]); // Right column
                Assert.AreEqual(255, image[(0 * stride + i) * bytesPerPixel + 2]); // Blue channel for the border
            }
        }
    }
}