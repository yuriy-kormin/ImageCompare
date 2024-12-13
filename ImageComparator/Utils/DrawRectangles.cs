namespace ImageComparator.Utils
{
    public class DrawRectangles
    {
        private static unsafe void SetColor(byte* resPtr, int address, List<int> color)
        {
            // Ensure the color list has exactly 3 elements for RGB
            if (color == null || color.Count != 3)
            {
                throw new ArgumentException("Color must have exactly 3 elements (R, G, B).");
            }

            resPtr[address] = (byte)color[2];    // Red
            resPtr[address + 1] = (byte)color[1]; // Green
            resPtr[address + 2] = (byte)color[0]; // Blue
        }

        public static unsafe void DrawBorder(
            byte* resPtr, 
            int x, 
            int y, 
            int width, 
            int height, 
            int stride, 
            int bytesPerPixel, 
            List<int>? color = null)
        {
            // Default color is red if not provided
            color ??= new List<int> { 255, 0, 0 };

            // Draw Top and Bottom borders
            DrawHorizontalBorderLine(resPtr, x, y, width, bytesPerPixel, stride, color);  // Top border
            DrawHorizontalBorderLine(resPtr, x, y + height - 1, width, bytesPerPixel, stride, color); // Bottom border

            // Draw Left and Right borders
            DrawVerticalBorderLine(resPtr, x, y, height, bytesPerPixel, stride, color);  // Left border
            DrawVerticalBorderLine(resPtr, x + width - 1, y, height, bytesPerPixel, stride, color); // Right border
        }

        private static unsafe void DrawHorizontalBorderLine(
            byte* resPtr, 
            int x, 
            int y, 
            int width, 
            int bytesPerPixel, 
            int stride, 
            List<int> color) 
        {
            for (int i = 0; i < width; i++)
            {
                int offset = (y * stride) + ((x + i) * bytesPerPixel);
                SetColor(resPtr, offset, color);
            }
        }

        private static unsafe void DrawVerticalBorderLine(
            byte* resPtr, 
            int x, 
            int y, 
            int height, 
            int bytesPerPixel, 
            int stride, 
            List<int> color) 
        {
            for (int j = 0; j < height; j++)
            {
                int offset = ((y + j) * stride) + (x * bytesPerPixel);
                SetColor(resPtr, offset, color);
            }
        }
    }
}
