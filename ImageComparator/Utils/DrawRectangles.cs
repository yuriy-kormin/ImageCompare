using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace ImageComparator.Utils
{
    public class DrawRectangles
    {   
        private static Pen DebugPen { get; } = new Pen(Settings.DebugRectColor, 1);
        private static Pen MainPen { get; } = new Pen(Settings.RectColor, 1);

        public static void DrawBorder(Bitmap bitmap, Rectangle rectangle, bool debug=false )
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
