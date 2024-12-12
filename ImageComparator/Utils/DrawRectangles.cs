
namespace ImageComparator.Utils
{
    public class DrawRectangles
    {
        private static unsafe void SetRed(byte* resPtr, int address,List<int> color)
        {
        
            resPtr[address] = (byte)color[0];
            resPtr[address+1] = (byte)color[1];
            resPtr[address+2] = (byte)color[2];
        }
    
        public static unsafe void DrawBorder(byte* resPtr, int x, int y, int width, int height, int stride, int bytesPerPixel,List<int>? color= null)
        {
            if (color == null)
            {
                color = new List<int>(){0,0,255};
            }
        
            // Top and Bottom borders
        
            for (int i = 0; i < width; i++)
            {
                int topOffset = (y * stride) + ((x + i) * bytesPerPixel);
                int bottomOffset = ((y + height - 1) * stride) + ((x + i) * bytesPerPixel);
            
                SetRed(resPtr, topOffset, color);
                SetRed(resPtr, bottomOffset,color);
            }

            // Left and Right borders
            for (int j = 0; j < height; j++)
            {
                int leftOffset = ((y + j) * stride) + (x * bytesPerPixel);
                int rightOffset = ((y + j) * stride) + ((x + width - 1) * bytesPerPixel);
            
                SetRed(resPtr, leftOffset,color);
                SetRed(resPtr, rightOffset,color);
            }
        }
    }    
}
