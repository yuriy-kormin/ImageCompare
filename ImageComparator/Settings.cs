using System.Drawing;

namespace ImageComparator;

/// <summary>
/// Holds global settings and configuration parameters for the image comparison process.
/// </summary>
public class Settings
{
    /// <summary>
    /// Enables or disables debug mode. If true, additional debug information is drawn.
    /// </summary>
    public static bool Debug { get; } = true;

    /// <summary>
    /// Pixel shift for the debug rectangle in debug mode.
    /// </summary>
    public static int DebugRectShiftPX { get; } = 3;

    /// <summary>
    /// Color of the debug rectangle borders.
    /// </summary>
    public static Color DebugRectColor { get; } = Color.Black;

    /// <summary>
    /// Color of the rectangles drawn around differing squares.
    /// </summary>
    public static Color RectColor { get; } = Color.Red;

    /// <summary>
    /// Determines whether to apply Gaussian blur to the images before comparison.
    /// </summary>
    public static bool applyGaussianBlur { get; } = false;

    /// <summary>
    /// Size of the square regions to compare in pixels.
    /// </summary>
    public static int squareSize { get; } = 15;

    /// <summary>
    /// Threshold percentage for pixel brightness comparison.
    /// </summary>
    public static int PixelCounterPercentageThreshold { get; } = 20;

    /// <summary>
    /// Sigma value used for the Gaussian blur filter.
    /// </summary>
    public static double GaussianSigma { get; } = 2.0f;

    /// <summary>
    /// Threshold for pixel brightness percentage; can be set dynamically.
    /// </summary>
    public static int PixelBrightPercentageThreshold { get; set; } = 10;

    /// <summary>
    /// Maximum number of differing regions (squares) to detect; -1 means no limit.
    /// </summary>
    public static int DiffCount { get; set; } = -1;
}