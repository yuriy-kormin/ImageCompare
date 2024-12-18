using System.Drawing;

namespace ImageComparator.Utils
{
    /// <summary>A utility class to draw rectangles on a bitmap for visual comparison.</summary>
    public class DrawRectangles
    {   
        /// <summary>Pen used for drawing debug rectangles (with debug color).</summary>
        private static Pen DebugPen { get; } = new Pen(Settings.DebugRectColor, 1);

        /// <summary>Pen used for drawing main rectangles (with the main color).</summary>
        private static Pen MainPen { get; } = new Pen(Settings.RectColor, 1);

        /// <summary>Draws a rectangle border on a bitmap at the specified location.</summary>
        /// 
        /// <param name="bitmap">The bitmap on which to draw the rectangle.</param>
        /// <param name="rectangle">The rectangle to draw on the bitmap.</param>
        /// <param name="debug">A flag indicating whether to draw the debug version of the rectangle (with a shifted position).</param>
        /// 
        /// <remarks>
        /// If the debug flag is set to true, the rectangle will be inflated inwards by the specified debug shift amount.
        /// </remarks>
        public static void DrawBorder(Bitmap bitmap, Rectangle rectangle, bool debug = false)
        {
            if (debug)
            {
                rectangle.Inflate(-Settings.DebugRectShiftPX, -Settings.DebugRectShiftPX);
            }

            Pen pen = debug ? DebugPen : MainPen;
            
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawRectangle(pen, rectangle); 
            }
        }
    }
}