using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace ImageComparator.Utils
{
    public class DrawRectangles
    {

        public static void DrawBorder(
            Bitmap bitmap,
            int x, int y, int dx, int dy,
            List<int>? color = null)
        {
            // Default color is red if not provided
            color ??= new List<int> { 255, 0, 0 };
            
            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (Pen pen = new Pen(Color.FromArgb(color[0],color[1],color[2]), 1))
            {
                graphics.DrawRectangle(pen, new Rectangle(x, y, dx, dy)); 
            }
            
        }
    }
}
